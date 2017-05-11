using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models
{
    public class IssuePriority
    {
        public int Id { get; set; }

        [DisplayName("Priority")]
        public string Name { get; set; }
        public ICollection<Issue> Issues { get; set; }
    }
}