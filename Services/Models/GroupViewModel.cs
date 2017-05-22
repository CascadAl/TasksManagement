using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class GroupViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Title should be from 2 to 50 characters long")]
        public string Title { get; set; }

        [StringLength(240, ErrorMessage = "Title should be less than 240 characters long")]
        public string Description { get; set; }

        public string Role { get; set; }
    }
}