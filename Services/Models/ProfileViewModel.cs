using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Path to avatar")]
        public string AvatarPath { get; set; }
    }
}
