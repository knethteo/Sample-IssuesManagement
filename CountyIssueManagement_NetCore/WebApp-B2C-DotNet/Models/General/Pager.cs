using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.Models.General
{
    public class Pager
    {
        public Pager(int pageSize, int page)
        {
            PageSize = pageSize;
            Page = page;
        }

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
        public int PreviousPage => Page - 1;
        public int NextPage => Page + 1;
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalRecords { get; set; }

        //public string TotalRecordsStatement => TotalRecords > 0
        //    ? TotalRecords + Resources.Common.RecordsFoundText
        //    : Resources.Common.NoRecordsFoundText;

        public int TotalPages
        {
            get
            {
                if (TotalRecords == 0)
                {
                    return 0;
                }

                if (TotalRecords < PageSize)
                {
                    return 1;
                }

                return (int)Math.Ceiling(TotalRecords / (double)PageSize);
            }
        }

        public IQueryable<T> ApplyPaging<T>(IQueryable<T> query)
        {
            TotalRecords = query.Count();

            if (Page < 1)
            {
                Page = 1;
            }

            if (TotalPages < Page)
            {
                Page = TotalPages;
                return query;
            }

            return query
                .Skip(PageSize * (Page - 1))
                .Take(PageSize);
        }
    }
}