using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp_OpenIDConnect_DotNet_B2C.Models;

namespace WebApp_OpenIDConnect_DotNet_B2C.DatabaseContext
{
    public class IssueManagementInitializer : DropCreateDatabaseIfModelChanges<IssueManagementDbContext>
    {
        protected override void Seed(IssueManagementDbContext context)
        {
            var statuses = new List<IssueStatus>()
            {
                new IssueStatus
                {
                    Name = "New"
                },
                new IssueStatus
                {
                    Name = "Accepted"
                },
                new IssueStatus
                {
                    Name = "Assigned"
                },
                new IssueStatus
                {
                    Name = "InProgress"
                },
                new IssueStatus
                {
                    Name = "OnHold"
                },
                new IssueStatus
                {
                    Name = "Completed"
                },
                new IssueStatus
                {
                    Name = "Duplicate"
                }
            };
            statuses.ForEach(status => context.IssueStatuses.Add(status));

            var categories = new List<IssueCategory>()
            {
                new IssueCategory
                {
                    Name = "Other Road Repair"

                },
                new IssueCategory
                {
                    Name = "Sidewalk Repair"

                },
                new IssueCategory
                {
                    Name = "Dead Animal removal"

                },
                new IssueCategory
                {
                    Name = "Abandoned Vehicle"

                },
                new IssueCategory
                {
                    Name = "Broken Parking Meter"

                },
                new IssueCategory
                {
                    Name = "Damaged Road Sign"

                },
                new IssueCategory
                {
                    Name = "Traffic Signal Damage/Issue"

                },
                new IssueCategory
                {
                    Name = "Street Light Repair"

                },
                new IssueCategory
                {
                    Name = "Snow Removal (Roadway)"

                },
                new IssueCategory
                {
                    Name = "Snow Removal (Sidewalk)"

                },
                new IssueCategory
                {
                    Name = "Street Sweeping"

                },
                new IssueCategory
                {
                    Name = "Tree/Shrub Pruning"

                },
                new IssueCategory
                {
                    Name = "Utility Repair"

                },
                new IssueCategory
                {
                    Name = "Other"
                }
            };
            categories.ForEach(category => context.IssueCategories.Add(category));

            var piorities = new List<IssuePriority>()
            {
                new IssuePriority
                {
                    Name = "Critical"
                },
                new IssuePriority
                {
                    Name = "High"
                },
                new IssuePriority
                {
                    Name = "Medium"
                },
                new IssuePriority
                {
                    Name = "Low"
                }
            };
            piorities.ForEach(piority => context.IssuePriorities.Add(piority));

            var issueFieldAgents = new List<IssueFieldAgent>()
            {
                new IssueFieldAgent
                {
                    Name = "Eric Williams",
                    Email = "eric.williams@microsoft.com"
                },
                new IssueFieldAgent
                {
                    Name = "Per Ostman",
                    Email = "postman@microsoft.com"
                },
                new IssueFieldAgent
                {
                    Name = "Kenneth Teo",
                    Email = "kennetht@microsoft.com"
                },
                new IssueFieldAgent
                {
                    Name = "Jagan Pavuluri",
                    Email = "japavulu@microsoft.com"
                },
                new IssueFieldAgent
                {
                    Name = "Arvind Ravindran",
                    Email = "arravin@microsoft.com"
                },
                new IssueFieldAgent
                {
                    Name = "Prashanth Shankhawaram",
                    Email = "prshankh@microsoft.com"
                }
            };
            issueFieldAgents.ForEach(fieldAgent => context.IssueFieldAgents.Add(fieldAgent));

            base.Seed(context);
        }
    }
}