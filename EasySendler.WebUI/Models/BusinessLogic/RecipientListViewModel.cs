using System.ComponentModel.DataAnnotations;

namespace EasySendler.Models.BusinessLogic
{
    public class RecipientListViewModel
    {
        public int rlId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}