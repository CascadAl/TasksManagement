using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
    public class MemberViewModel
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public int RoleId { get; set; }

        public DateTime JoinedAt { get; set; }

        public bool IsOwner()
        {
            return String.Equals(Role, "Owner");
        }
    }
}