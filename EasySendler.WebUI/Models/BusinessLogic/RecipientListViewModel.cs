using System.ComponentModel.DataAnnotations;

namespace EasySendler.Models.BusinessLogic
{
    public class RecipientListViewModel
    {
        public int rlId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }
    }
}