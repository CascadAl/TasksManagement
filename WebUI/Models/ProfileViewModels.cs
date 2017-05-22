using Services.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebUI.Models
{
    public class ProfileDetailsViewModel
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

        public int AssignedTasks { get; set; }
    }

    public class EditProfileViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full name")]
        [StringLength(50)]
        public string FullName { get; set; }

        [Display(Name = "Username")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Path to avatar")]
        public string AvatarPath { get; set; }

        [Display(Name = "Upload Image")]
        [ValidateImage]
        public HttpPostedFileBase Avatar { get; set; }
    }
}
