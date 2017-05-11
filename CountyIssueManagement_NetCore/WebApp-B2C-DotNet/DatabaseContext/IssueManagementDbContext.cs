namespace WebApp_OpenIDConnect_DotNet_B2C.DatabaseContext
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class IssueManagementDbContext : DbContext
    {
        public IssueManagementDbContext() : base("name=IssueManagementDbContext")
        {
            //Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<IssueManagementDbContext>(new IssueManagementInitializer());
        }

        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<IssueFieldAgent> IssueFieldAgents { get; set; }
        public virtual DbSet<IssueStatus> IssueStatuses { get; set; }
        public virtual DbSet<IssueCategory> IssueCategories { get; set; }
        public virtual DbSet<IssuePriority> IssuePriorities { get; set; }

        public System.Data.Entity.DbSet<WebApp_OpenIDConnect_DotNet_B2C.Models.test4Jagan> test4Jagan { get; set; }

        public System.Data.Entity.DbSet<WebApp_OpenIDConnect_DotNet_B2C.Models.test4Kenneth> test4Kenneth { get; set; }
    }
}