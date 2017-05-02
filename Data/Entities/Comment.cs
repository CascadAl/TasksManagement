using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Comment : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int IssueId { get; set; }

        [Required]
        public bool IsEdited { get; set; }

        public DateTime? LastEditedAt { get; set; }


        public virtual ApplicationUser User { get; set; }

        public virtual Issue Issue { get; set; }
    }
}