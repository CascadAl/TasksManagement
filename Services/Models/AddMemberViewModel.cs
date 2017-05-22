using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class AddMemberViewModel
    {
        public ICollection<GroupRole> GroupRoles { get; set; }

        public ICollection<MemberViewModel> Members { get; set; }

        public string GroupTitle { get; set; }

        public int GroupId { get; set; }

        [Required(ErrorMessage ="Select user using search by first or last name")]
        public int? UserId { get; set; }

        [Required(ErrorMessage ="Select member role from the dropdown list")]
        public int RoleId { get; set; }

        public bool IsOwner { get; set; }

        public AddMemberViewModel()
        {
            GroupRoles = new List<GroupRole>();
            Members = new List<MemberViewModel>();
        }
    }
}
