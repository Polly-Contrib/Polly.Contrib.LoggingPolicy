using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.LoggingPolicy;

namespace ConsoleAppExampleForBlogPost
{
    public class FooClient
    {
        private readonly ILogger logger;
        private readonly HttpClient client;

        public FooClient(ILogger<FooClient> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public async Task<string> GetStringAsync(string url, CancellationToken token)
        {
            var context = new Context { ["url"] = url }.WithLogger(logger);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.SetPolicyExecutionContext(context);

            var response = await client.SendAsync(request, token);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
