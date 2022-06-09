using Dive.App.Models;
using Dive.App.ViewModels;
using Xunit;

namespace Dive.Tests.Unit.ViewModels
{
    public class PostViewModelsTest
    {
        [Fact]
        public void markdown_elements_are_removed_from_the_body()
        {
            var post = new Post
            {
                Body = "Lorem **ipsum**",
            };
            var viewModel = new PostViewModel { Post = post };

            var summary = viewModel.Summary();

            Assert.Equal("Lorem ipsum", summary);
        }
    }
}
