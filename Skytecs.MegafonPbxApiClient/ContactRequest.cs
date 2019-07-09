using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Skytecs.MegafonPbxApiClient
{
    internal class ContactRequest : CallbackRequest, IMegafonContactRequest
    {
        public ContactRequest(IFormCollection requestForm) : base(requestForm)
        {
            if (requestForm == null)
            {
                throw new ArgumentNullException(nameof(requestForm));
            }

            Phone = requestForm["phone"].Single();
            CallId = requestForm["callid"].Single();
        }

        public string Phone { get; }
        public string CallId { get; }
    }
}