using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class AddMemberProfileViewModel
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }
    }
}
