using HtmlAgilityPack;
using System.Net.Http;
using Xunit;

namespace Dive.Tests.Integration
{
    public class TestUtilities
    {
        public static string ResponseUrl(HttpResponseMessage response)
        {
            return response.RequestMessage.RequestUri.PathAndQuery.ToString();
        }

        public static void AssertHtmlContains(string assert, string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var result = document.DocumentNode.SelectNodes("//text()[contains(., '" + assert + "')]/..");

            Assert.NotNull(result);
        }
    }
}