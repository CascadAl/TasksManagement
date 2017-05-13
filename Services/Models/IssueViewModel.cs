using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class IssueViewModel
    {
        public int? Id { get; set; }

        public int GroupId { get; set; }

        public int IssueNumber { get; set; }

        public string Text { get; set; }

        [Required(ErrorMessage ="Issue title is required")]
        [StringLength(150, MinimumLength =3, ErrorMessage = "Title should be from 3 to 150 characters long")]
        public string Title { get; set; }

        public DateTime OpenedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public int? AssignedToUserId { get; set; }

        public int OpenedByUserId { get; set; }

        public string OpenedByUser { get; set; }

        public string ClosedByUser { get; set; }
    }
}
