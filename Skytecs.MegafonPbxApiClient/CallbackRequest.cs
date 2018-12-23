using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Skytecs.MegafonPbxApiClient
{
    public abstract class CallbackRequest
    {
        public CallbackRequest(IFormCollection requestForm)
        {
            if (requestForm == null)
            {
                throw new System.ArgumentNullException(nameof(requestForm));
            }

            CrmToken = requestForm["crm_token"].Single();

            Cmd = requestForm["cmd"].Single();
        }

        public string CrmToken { get; }
        public string Cmd { get; }
    }
}