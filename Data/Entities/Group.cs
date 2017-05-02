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

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int IssuesTotal { get; set; }

        public virtual ICollection<GroupMember> Members { get; set; }

        public virtual ICollection<Issue> Issues { get; set; }

        public Group()
        {
            Issues=new List<Issue>();
            Members = new List<GroupMember>();
        }
    }
}
