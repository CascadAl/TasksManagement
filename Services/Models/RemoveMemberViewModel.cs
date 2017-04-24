using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class RemoveMemberViewModel
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        public int UserToRemove { get; set; }
    }
}
