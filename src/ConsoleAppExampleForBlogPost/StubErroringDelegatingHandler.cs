using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppExampleForBlogPost
{
    public class StubErroringDelegatingHandler : DelegatingHandler
    {
        private readonly Random random;

        public StubErroringDelegatingHandler()
        {
            random = new Random();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            switch (random.Next(3))
            {
                case 1:
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                case 2:
                    throw new HttpRequestException();

                default:
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK){Content = new StringContent("Dummy content from the stub helper class.")});
            }
        }
    }
}
