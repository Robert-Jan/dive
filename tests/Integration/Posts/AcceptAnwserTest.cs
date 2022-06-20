using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Xunit;

namespace Dive.Tests.Integration.Posts
{
    public class AcceptAnwserTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Guest { get; }

        public HttpClient LoggedIn { get; }

        public DiveContext Context { get; }

        public AcceptAnwserTest(TestApplicationFixture app)
        {
            Guest = app.CreateClient();
            LoggedIn = app.CreateAuthenticatedClient();
            Context = app.CreateContext();
        }

        [Fact]
        public async Task a_anwser_can_be_accepted_as_the_correct_anwser()
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = 1 };
            var anwser = new Post { Parent = post, Body = "Example anwser", UserId = 2 };
            Context.Posts.Add(post);
            Context.Posts.Add(anwser);
            Context.SaveChanges();

            var response = await LoggedIn.GetAsync($"/questions/{post.Id}/accept/{anwser.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Context.Entry(post).Reload();
            Assert.Equal(anwser.Id, post.AcceptedAnswerId);
        }

        [Fact]
        public async Task only_the_question_user_can_accept_a_anwser()
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = 2 };
            var anwser = new Post { Parent = post, Body = "Example anwser", UserId = 1 };
            Context.Posts.Add(post);
            Context.Posts.Add(anwser);
            Context.SaveChanges();

            var response = await LoggedIn.GetAsync($"/questions/{post.Id}/accept/{anwser.Id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task a_guest_cannot_accept_a_anwser()
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = 1 };
            var anwser = new Post { Parent = post, Body = "Example anwser", UserId = 2 };
            Context.Posts.Add(post);
            Context.Posts.Add(anwser);
            Context.SaveChanges();

            var response = await Guest.GetAsync($"/questions/{post.Id}/accept/{anwser.Id}");

            Assert.Equal($"/login?ReturnUrl=%2Fquestions%2F{post.Id}%2Faccept%2F{anwser.Id}", TestUtilities.ResponseUrl(response));
        }
    }
}