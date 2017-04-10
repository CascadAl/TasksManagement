using Microsoft.AspNet.Identity.EntityFramework;
using Data;

namespace Data.Entities
{
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>, IEntity
    {
        
    }
}