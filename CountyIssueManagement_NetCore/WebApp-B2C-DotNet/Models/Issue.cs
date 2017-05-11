using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public int IssueCategoryId { get; set; }
        public IssueCategory IssueCategory { get; set; }
        public int? IssuePriorityId { get; set; }
        public IssuePriority IssuePriority { get; set; }
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public int IssueStatusId { get; set; }
        public IssueStatus IssueStatus { get; set; }
        public string Eta { get; set; }
        public string Creator { get; set; }
        public string InternalComment { get; set; }
        public int? IssueFieldAgentId { get; set; }
        public IssueFieldAgent IssueFieldAgent { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public string ImageUrl { get; set; }

    }
}