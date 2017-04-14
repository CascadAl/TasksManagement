using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class RoleType :IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public virtual ICollection<ApplicationRole> Roles { get; set; }

        public RoleType()
        {
            Roles=new List<ApplicationRole>();
        }
    }
}
