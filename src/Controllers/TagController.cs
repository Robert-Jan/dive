using Dive.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Dive.App.Repositories;
using System.Threading.Tasks;

namespace Dive.App.Controllers
{
    public class TagController : BaseController
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet("/tags")]
        public async Task<IActionResult> Index()
        {
            return View(new TagViewModel
            {
                Tags = await _tagRepository.GetAllTagsAsync()
            });
        }
    }
}
