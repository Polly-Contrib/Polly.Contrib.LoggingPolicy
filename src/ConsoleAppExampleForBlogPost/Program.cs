using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.LoggingPolicy;

namespace ConsoleAppExampleForBlogPost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole());

            void LogAction(ILogger logger, Context context, DelegateResult<HttpResponseMessage> outcome)
            {
                // This logging can be as sophisticated as you like; this is a simple example to illustrate principles.
                string url = context.GetValueOrDefault("url").ToString() ?? "[unknown]";
                string happened = outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString();
                logger?.LogInformation($"{url}: {happened}");
            }

            services.AddHttpClient<FooClient>()
                
                // Whatever standard resilience logic you want.
                .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(5))
                
                // Logging policy to log transient faults as they occur.
                // The logging policy is inside the retry policy, so will log faults from each try, before they are retried.
                .AddTransientHttpErrorPolicy(policy => policy.AsyncLog(ctx => ctx.GetLogger(), LogAction))
                
                // For this test example we add a stub DelegatingHandler which will manufacture faults for us.
                .AddHttpMessageHandler(() => new StubErroringDelegatingHandler());


            // Quickly test our FooClient.
            var fooClient = services
                .BuildServiceProvider()
                .GetRequiredService<FooClient>();

            string result = await fooClient.GetStringAsync("https://www.google.com", default(CancellationToken));

            Console.WriteLine($"Got result: {result}");

            Console.ReadKey();
        }
    }
}