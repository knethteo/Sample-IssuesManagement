using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models
{
    public class IssueStatus
    {
        public int Id { get; set; }

        [DisplayName("Status")]
        public string Name { get; set; }
        public ICollection<Issue> Issues { get; set; }
    }
}