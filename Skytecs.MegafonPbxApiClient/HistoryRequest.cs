using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Skytecs.MegafonPbxApiClient
{
    internal class HistoryRequest : CallbackRequest, IMegafonHistoryRequest
    {
        public HistoryRequest(IFormCollection requestForm) : base(requestForm)
        {
            if (requestForm == null)
            {
                throw new ArgumentNullException(nameof(requestForm));
            }

            var type = requestForm["type"].Single();
            if (type == "in")
            {
                Type = MegafonCallType.Incoming;
            }
            else if (type == "out")
            {
                Type = MegafonCallType.Outgoing;
            }

            User = requestForm["user"].Single();

            Ext = requestForm["ext"].SingleOrDefault();

            GroupRealName = requestForm["groupRealName"].SingleOrDefault();
            TelNum = requestForm["telnum"].SingleOrDefault();
            Phone = requestForm["phone"].Single();
            Diversion = requestForm["diversion"].SingleOrDefault();
            Start = DateTime.Parse(requestForm["start"].Single(), new DateTimeFormatInfo { FullDateTimePattern = "YYYYmmddTHHMMSSZ" });
            Duration = int.Parse(requestForm["duration"].Single());
            CallId = requestForm["callid"].Single();
            Link = requestForm["link"].Single();

            switch(requestForm["link"].Single().ToLowerInvariant())
            {
                case "success":
                    Status = MegafonCallResult.Success;
                    break;
                case "missed":
                    Status = MegafonCallResult.Missed;
                    break;
                case "busy":
                    Status = MegafonCallResult.Busy;
                    break;
                case "notavailable":
                    Status = MegafonCallResult.NotAvailable;
                    break;
                case "notallowed":
                    Status = MegafonCallResult.NotAllowed;
                    break;
            }
        }
        public MegafonCallType Type { get; }
        public string User { get; }
        public string Ext { get; }
        public string GroupRealName { get; }
        public string TelNum { get; }
        public string Phone { get; }
        public string Diversion { get; }
        public DateTime Start { get; }
        public int Duration { get; }
        public string CallId { get; }
        public string Link { get; }
        public MegafonCallResult Status { get; }
    }
}