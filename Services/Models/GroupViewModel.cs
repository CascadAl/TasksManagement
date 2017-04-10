using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class GroupViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }

        public string Description { get; set; }
        
        
    }
}