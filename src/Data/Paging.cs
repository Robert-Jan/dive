using System;
using System.Collections.Generic;

namespace Dive.App.Data
{
    public class PagingMeta
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }

        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
    }

    public class PagedResult<T> where T : class
    {
        public IList<T> Results { get; set; }

        public PagingMeta Meta { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
            Meta = new PagingMeta();
        }
    }
}