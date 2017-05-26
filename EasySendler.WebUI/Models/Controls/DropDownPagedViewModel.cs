using System.Collections.Generic;

namespace EasySendler.Models.Controls
{
    public class DropDownPagedViewModel
    {
        public int Total { get; set; }
        public List<DropDownViewModel> Results { get; set; }
    }
}