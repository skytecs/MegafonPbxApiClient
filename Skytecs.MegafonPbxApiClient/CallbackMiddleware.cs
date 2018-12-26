using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Skytecs.MegafonPbxApiClient
{
    class CallbackMiddleware
    {
        private readonly MegafonCallbackOptions _options;
        private readonly ILogger<CallbackMiddleware> _logger;

        public CallbackMiddleware(MegafonCallbackOptions options,
            ILogger<CallbackMiddleware> logger = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? new VoidLogger<CallbackMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context, string callbackToken)
        {

            if (_options == null)
            {
                throw new ArgumentNullException(nameof(_options));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request.Method != "POST")
            {
                await SendResponse(context.Response, 400, "{ \"error\": \"Invalid request\" }");

                return;
            }

            if(context.Request.Form["crm_token"] != callbackToken)
            {
                await SendResponse(context.Response, 401, "{ \"error\": \"Invalid token\" }");

                return;
            }

            try
            {
                var requestForm = await context.Request.ReadFormAsync();

                var cmd = requestForm["cmd"];

                switch (cmd)
                {
                    case "history":
                        await _options.InvokeOnHistory(new HistoryRequest(requestForm));
                        break;
                    case "event":
                        await _options.InvokeOnEvent(new EventRequest(requestForm));
                        break;
                    case "contact":
                        var contact = await _options.InvokeOnContact(new ContactRequest(requestForm));

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

        private async Task SendResponse(HttpResponse response, int code, string json)
        {
            response.StatusCode = code;
            response.ContentType = "application/json";
            await response.WriteAsync(json);
        }
    }
}
