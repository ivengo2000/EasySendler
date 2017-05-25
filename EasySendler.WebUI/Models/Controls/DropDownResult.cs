using Newtonsoft.Json;

namespace EasySendler.Models.Controls
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DropDownResult
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}