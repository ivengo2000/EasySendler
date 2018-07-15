using Newtonsoft.Json;

namespace EasySendler.Models.Controls
{
    public class MobileListViewModel: DropDownViewModel
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}