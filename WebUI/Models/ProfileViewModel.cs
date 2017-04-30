using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class ProfileViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [StringLength(50)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Path to avatar")]
        public string AvatarPath { get; set; }
    }
}
