using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
    public class MemberViewModel
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }
    }
}