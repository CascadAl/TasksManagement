using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class GroupMemberViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
