using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dive.App.Data;
using Dive.App.Models;
using Xunit;

namespace Dive.Tests.Integration.Posts
{
    public class VoteTest : IClassFixture<TestApplicationFixture>
    {
        public HttpClient Guest { get; }

        public HttpClient LoggedIn { get; }

        public DiveContext Context { get; }

        public VoteTest(TestApplicationFixture app)
        {
            Guest = app.CreateClient();
            LoggedIn = app.CreateAuthenticatedClient();
            Context = app.CreateContext();
        }

        [Theory]
        [InlineData(VoteType.Upvote, 1)]
        [InlineData(VoteType.Downvote, -1)]
        public async Task a_user_can_cast_a_vote(VoteType vote, int expectedScore)
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = Context.Users.First().Id };
            Context.Posts.Add(post);
            Context.SaveChanges();

            var response = await LoggedIn.GetAsync($"/questions/{post.Id}/vote/{vote}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("{\"score\":" + expectedScore + ",\"vote\":" + (int)vote + "}", await response.Content.ReadAsStringAsync());
        }

        [Theory]
        [InlineData(VoteType.Upvote)]
        [InlineData(VoteType.Downvote)]
        public async Task a_user_can_remove_a_vote(VoteType vote)
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = Context.Users.First().Id };
            Context.Posts.Add(post);
            Context.SaveChanges();
            Context.Votes.Add(new Vote { PostId = post.Id, UserId = Context.Users.First().Id, Type = vote });
            Context.SaveChanges();

            var response = await LoggedIn.GetAsync($"/questions/{post.Id}/vote/{vote}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("{\"score\":0,\"vote\":null}", await response.Content.ReadAsStringAsync());
        }

        [Theory]
        [InlineData(VoteType.Upvote, VoteType.Downvote, -1)]
        [InlineData(VoteType.Downvote, VoteType.Upvote, 1)]
        public async Task a_user_can_change_a_vote(VoteType givenVote, VoteType newVote, int expectedScore)
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = Context.Users.First().Id };
            Context.Posts.Add(post);
            Context.SaveChanges();
            Context.Votes.Add(new Vote { PostId = post.Id, UserId = Context.Users.First().Id, Type = givenVote });
            Context.SaveChanges();

            var response = await LoggedIn.GetAsync($"/questions/{post.Id}/vote/{newVote}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("{\"score\":" + expectedScore + ",\"vote\":" + (int)newVote + "}", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task a_guest_cannot_cast_a_vote()
        {
            var post = new Post { Title = "Example title", Body = "Example body", UserId = Context.Users.First().Id };
            Context.Posts.Add(post);
            Context.SaveChanges();

            var response = await Guest.GetAsync($"/questions/{post.Id}/vote/1");

            Assert.Equal($"/login?ReturnUrl=%2Fquestions%2F{post.Id}%2Fvote%2F1", TestUtilities.ResponseUrl(response));
        }
    }
}