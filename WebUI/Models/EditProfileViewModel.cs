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
    public class EditProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

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
