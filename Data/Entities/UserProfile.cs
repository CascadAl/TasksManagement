using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserProfile : IEntity
    {
        [Key, ForeignKey(nameof(ApplicationUser))]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string AvatarName { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
