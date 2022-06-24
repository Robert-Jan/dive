using System.Threading.Tasks;
using Dive.App.Repositories;
using Dive.App.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dive.App.Controllers
{
    public class SearchController : BaseController
    {
        private readonly ISearchRepository _searchRepository;

        public SearchController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        [HttpGet("/search")]
        public async Task<IActionResult> Index(string q = "", int page = 1)
        {
            return View(new SearchViewModel
            {
                Posts = await _searchRepository.GetPagedResultAsync(q, page)
            });
        }
    }
}