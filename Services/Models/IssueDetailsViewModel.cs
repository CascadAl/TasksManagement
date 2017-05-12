using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class IssueDetailsViewModel
    {
        public IssueViewModel Issue { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
