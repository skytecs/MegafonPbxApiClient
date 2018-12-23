using Newtonsoft.Json;

namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// Сотрудник Облачной АТС
    /// </summary>
    public class MegafonAccount
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("realName")]
        public string RealName { get; set; }
        [JsonProperty("ext")]
        public string Ext { get; set; }
        [JsonProperty("telnum")]
        public string TelNum { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}