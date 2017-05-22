using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataTransferObjects
{
    public class ProfileDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string AvatarPath { get; set; }
    }
}
