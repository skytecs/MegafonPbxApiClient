using Newtonsoft.Json;

namespace Skytecs.MegafonPbxApiClient
{
    /// <summary>
    /// Информация об отделе
    /// </summary>
    public class MegafonGroup
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("realName")]
        public string RealName { get; set; }
        [JsonProperty("ext")]
        public string Ext { get; set; }
    }
}