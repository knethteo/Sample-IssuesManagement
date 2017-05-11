using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models
{
    public class IssueFieldAgent
    {
        public int Id { get; set; }

        [DisplayName("FieldAgent")]
        public string Name { get; set; }

        public string Email { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}