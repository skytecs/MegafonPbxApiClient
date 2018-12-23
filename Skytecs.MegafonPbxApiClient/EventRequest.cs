using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Skytecs.MegafonPbxApiClient
{
    internal class EventRequest : CallbackRequest, IMegafonEventRequest
    {
        public EventRequest(IFormCollection requestForm) : base(requestForm)
        {
            if (requestForm == null)
            {
                throw new ArgumentNullException(nameof(requestForm));
            }

            switch (requestForm["type"].Single().ToLowerInvariant())
            {
                case "incoming":
                    Type = MegafonEventType.Incoming;
                    break;
                case "accepted":
                    Type = MegafonEventType.Accepted;
                    break;
                case "completed":
                    Type = MegafonEventType.Completed;
                    break;
                case "cancelled":
                    Type = MegafonEventType.Cancelled;
                    break;
                case "outgoing":
                    Type = MegafonEventType.Outgoing;
                    break;
                default:
                    Type = MegafonEventType.Unknown;
                    break;
            }

            User = requestForm["user"].Single();
            Ext = requestForm["ext"].SingleOrDefault();
            GroupRealName = requestForm["groupRealName"].SingleOrDefault();
            TelNum = requestForm["telnum"].SingleOrDefault();
            Phone = requestForm["phone"].Single();
            CallId = requestForm["callid"].Single();
        }

        public string User { get; }

        public string Ext { get; }

        public string GroupRealName { get; }

        public string TelNum { get; }

        public string Phone { get; }

        public string Diversion { get; }

        public string CallId { get; }

        public MegafonEventType Type { get; }
    }
}