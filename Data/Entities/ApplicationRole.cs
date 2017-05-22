using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using Data;

namespace Data.Entities
{
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>, IEntity
    {
        [Required]
        public int RoleTypeId { get; set; }

        public virtual RoleType RoleType { get; set; }
    }
}