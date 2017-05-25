using System.Collections.Generic;

namespace EasySendler.Models.Controls
{
    public class DropDownPagedResult
    {
        public int Total { get; set; }
        public List<DropDownResult> Results { get; set; }
    }
}