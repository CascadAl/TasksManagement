using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class IssueListViewModel
    {
        public IEnumerable<IssueViewModel> Issues { get; set; }

        public bool IsOwner { get; set; }

        public string GroupTitle { get; set; }
    }
}
