using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skytecs.MegafonPbxApiClient
{
    abstract class CallbackMiddleware<TOptions> : ICallbackMiddleware
    {
        private readonly ILogger<CallbackMiddleware<TOptions>> _logger;
        private readonly TOptions _options;

        public CallbackMiddleware(TOptions options, ILogger<CallbackMiddleware<TOptions>> logger = null)
        {
            _options = options;
            _logger = logger ?? new VoidLogger<CallbackMiddleware<TOptions>>();
        }

        public async Task InvokeAsync(HttpContext context, string callbackToken)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request.Method != "POST")
            {
                await SendResponse(context.Response, 400, "{ \"error\": \"Invalid request\" }");

                return;
            }

            try
            {
                var receivedToken = context.Request.Form["crm_token"].FirstOrDefault();
                if (string.Compare(receivedToken, callbackToken, true) != 0)
                {
                    await SendResponse(context.Response, 401, "{ \"error\": \"Invalid token\" }");

                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid or unreadable Megafon callback token");
                return;
            }

            try
            {
                var requestForm = await context.Request.ReadFormAsync();

                var cmd = requestForm["cmd"];

                switch (cmd)
                {
                    case "history":
                        await InvokeOnHistory(_options, new HistoryRequest(requestForm));
                        break;
                    case "event":
                        await InvokeOnEvent(_options, new EventRequest(requestForm));
                        break;
                    case "contact":
                        MegafonContact contact = await InvokeOnContact(_options, new ContactRequest(requestForm));

                        if (contact == null)
                        {
                            await SendResponse(context.Response, 200, "{}");
                        }
                        else
                        {
                            var json = $"{{contact_name:\"{contact.ContactName}\", responsible:\"{contact.Responsible}\"}}";
                            await SendResponse(context.Response, 200, json);
                        }

                        break;
                    default:
                        _logger.LogError($"Unknown callback");
                        break;
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                await SendResponse(context.Response, 400, "{ \"error\": \"Invalid parameters\" }");
            }
        }

        protected abstract Task<MegafonContact> InvokeOnContact(TOptions options, ContactRequest request);
        protected abstract Task InvokeOnEvent(TOptions options, EventRequest request);
        protected abstract Task InvokeOnHistory(TOptions options, HistoryRequest request);


        private async Task SendResponse(HttpResponse response, int code, string json)
        {
            response.StatusCode = code;
            response.ContentType = "application/json";
            await response.WriteAsync(json);
        }
    }

    class AdHocCallbackMiddleware : CallbackMiddleware<MegafonCallbackOptions>
    {
        public AdHocCallbackMiddleware(MegafonCallbackOptions options, ILogger<CallbackMiddleware<MegafonCallbackOptions>> logger = null) : base(options, logger)
        {
        }

        protected override async Task<MegafonContact> InvokeOnContact(MegafonCallbackOptions options, ContactRequest request)
        {
            return await options.InvokeOnContact(request);
        }

        protected override async Task InvokeOnEvent(MegafonCallbackOptions options, EventRequest request)
        {
            await options.InvokeOnEvent(request);
        }

        protected override async Task InvokeOnHistory(MegafonCallbackOptions options, HistoryRequest request)
        {
            await options.InvokeOnHistory(request);
        }
    }

    class BoundCallbackMiddleware<THandler> : CallbackMiddleware<MegafonCallbackOptions<THandler>>
    {
        private readonly THandler _handler;

        public BoundCallbackMiddleware(THandler handler, MegafonCallbackOptions<THandler> options, ILogger<BoundCallbackMiddleware<THandler>> logger = null) : base(options, logger)
        {
            _handler = handler;
        }

        protected override Task<MegafonContact> InvokeOnContact(MegafonCallbackOptions<THandler> options, ContactRequest request)
        {
            return options.InvokeOnContact(_handler, request);
        }

        protected override Task InvokeOnEvent(MegafonCallbackOptions<THandler> options, EventRequest request)
        {
            return options.InvokeOnEvent(_handler, request);
        }

        protected override Task InvokeOnHistory(MegafonCallbackOptions<THandler> options, HistoryRequest request)
        {
            return options.InvokeOnHistory(_handler, request);
        }
    }
}
