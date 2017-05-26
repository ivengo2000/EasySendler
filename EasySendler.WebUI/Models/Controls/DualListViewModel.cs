using Newtonsoft.Json;

namespace EasySendler.Models.Controls
{
    public class DualListViewModel : DropDownViewModel
    {
        [JsonProperty(PropertyName = "selected")]
        public int IsSelected { get; set; }
    }
}