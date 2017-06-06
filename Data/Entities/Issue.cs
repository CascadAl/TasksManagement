using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Issue: IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int IssueNumber { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime OpenedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        [Required]
        public int OpenedByUserId { get; set; }

        public int? ClosedByUserId { get; set; }


        public virtual Group Group { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<GroupMember> GroupMembers { get; set; }

        public virtual ApplicationUser UserOpened { get; set; }

        public virtual ApplicationUser UserClosed { get; set; }

    }
}
