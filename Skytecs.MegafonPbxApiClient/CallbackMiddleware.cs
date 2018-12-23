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
        private readonly RequestDelegate _next;

        public CallbackMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, 
            MegafonCallbackOptions callback,
            ILogger<CallbackMiddleware> logger = null)
        {

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }


            if (logger == null)
            {
                logger = new VoidLogger<CallbackMiddleware>();
            }

            if (PathString.FromUriComponent(callback.CallbackEndpoint) == context.Request.Path)
            {
                if (context.Request.Form["crm_token"] == callback.CallbackToken)
                {
                    try
                    {
                        var requestForm = await context.Request.ReadFormAsync();

                        var cmd = requestForm["cmd"];


                        switch (cmd)
                        {
                            case "history":
                                await callback.InvokeOnHistory(new HistoryRequest(requestForm));
                                break;
                            case "event":
                                await callback.InvokeOnEvent(new EventRequest(requestForm));
                                break;
                            case "contact":
                                var contact = await callback.InvokeOnContact(new ContactRequest(requestForm));

                                if(contact == null)
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
                                logger.LogError($"Unknown callback");
                                break;
                        };
                    }
                    catch(Exception e)
                    {
                        logger.LogError(e, e.Message);

                        await SendResponse(context.Response, 400, "{ \"error\": \"Invalid parameters\" }");

                    }
                }
                else
                {
                    await SendResponse(context.Response, 401, "{ \"error\": \"Invalid token\" }");
                }
            }
            else if (_next != null)
            {
                await _next.Invoke(context);
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
