using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class GroupViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public string Role { get; set; }
    }
}