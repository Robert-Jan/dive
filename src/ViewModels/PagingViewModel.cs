using Dive.App.Data;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Dive.App.ViewModels
{
    public class PagingViewModel
    {
        public PagingMeta Meta { get; set; }

        private static HttpContext HttpContext => new HttpContextAccessor().HttpContext;

        public string Url(int page)
        {
            var queryString = HttpContext.Request.QueryString.Value;
            var query = HttpUtility.ParseQueryString(queryString);

            query.Remove("page");
            query.Add("page", page.ToString());

            return HttpUtility.UrlDecode($"{HttpContext.Request.Path}?{query}");
        }

        public string PreviousUrl()
        {
            if (Meta.CurrentPage == 1) return "#";

            return Url(Meta.CurrentPage - 1);
        }

        public string NextUrl()
        {
            if (Meta.CurrentPage == Meta.PageCount) return "#";

            return Url(Meta.CurrentPage + 1);
        }

        public int[] SurroundingPages()
        {
            if (Meta.CurrentPage <= 2) return new int[] { 2, 3, 4 };
            if (Meta.CurrentPage >= Meta.PageCount - 1) return new int[] { Meta.PageCount - 3, Meta.PageCount - 2, Meta.PageCount - 1 };

            return new int[] { Meta.CurrentPage - 1, Meta.CurrentPage, Meta.CurrentPage + 1 };
        }
    }
}