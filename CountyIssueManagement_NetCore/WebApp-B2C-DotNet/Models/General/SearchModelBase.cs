//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using WebApp_OpenIDConnect_DotNet_B2C.Models;
//using WebApp_OpenIDConnect_DotNet_B2C.DatabaseContext;
//namespace WebApp_OpenIDConnect_DotNet_B2C.Models.General
//{
//    public class SearchModelBase<T> where T : class
//    {
//        protected const int PageSize = 24;

//        protected SearchModelBase()
//        {
//            Pager = new Pager(PageSize, 1);
//            Sorts = new SelectList(new List<SelectListItem>());
//        }

//        public IEnumerable<T> Records { get; set; }

//        public IEnumerable<SelectListItem> Sorts { get; protected set; }

//        public string Sort { get; set; }

//        public Pager Pager { get; }

//        protected SystemUserPrincipal CurrentUser { get; private set; }

//        public virtual void Execute(IssueManagementDbContext dataContext, SystemUserPrincipal currentUser, bool applyPaging)
//        {
//            CurrentUser = currentUser;
//            var query = BuildSearchQuery(dataContext);
//            query = ApplySecurity(query, currentUser, dataContext);
//            query = ApplyFilters(query);
//            query = ApplySort(query);
//            query = applyPaging ? Pager.ApplyPaging(query) : query;
//            Records = query.ToList();
//            SetLookups(dataContext);
//        }

//        protected abstract IQueryable<T> BuildSearchQuery(IssueManagementDbContext dataContext);

//        protected abstract IQueryable<T> ApplySecurity(IQueryable<T> query, SystemUserPrincipal currentUser, IssueManagementDbContext dataContext);

//        protected abstract IQueryable<T> ApplyFilters(IQueryable<T> query);

//        protected abstract IQueryable<T> ApplySort(IQueryable<T> query);

//        public virtual void SetLookups(IssueManagementDbContext dataContext)
//        {
//        }
//    }
//}