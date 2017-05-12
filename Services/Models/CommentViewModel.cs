using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class CommentViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        [Required]
        public int IssueId { get; set; }

        public bool IsEdited { get; set; }

        public DateTime? LastEditedAt { get; set; }

    }
}