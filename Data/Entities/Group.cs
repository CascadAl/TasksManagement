using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Group : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }


        public virtual ICollection<ApplicationUser> Users { get; set; }

        //public ICollection<GroupUserRole> GroupUserRoles { get; set; }

        public Group()
        {
            Users=new List<ApplicationUser>();
            //GroupAdmins=new List<GroupUserRole>();
        }
    }
}
