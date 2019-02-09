using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Polly.Contrib.LoggingPolicy.Specs
{
    public class LoggingPolicyTResultSpecs
    {
        [Fact]
        public void PolicyCallsTheLoggerIfPolicyHandlesException()
        {
            ILogger expectedLogger = new StubLogger();
            Func<Context, ILogger> loggerProvider = _ => expectedLogger;

            ILogger invokedLogger = null;
            Exception exceptionInvokedFor = null;
            HttpStatusCode? resultInvokedFor = null;
            Action<ILogger, Context, DelegateResult<HttpStatusCode>> logAction = (logger, context, outcome) =>
            {
                invokedLogger = logger;
                exceptionInvokedFor = outcome.Exception;
                resultInvokedFor = outcome.Result;
            };

            LoggingPolicy<HttpStatusCode> policy = Policy<HttpStatusCode>
                .Handle<TaskCanceledException>()
                .OrResult(r => r != HttpStatusCode.OK)
                .Log(loggerProvider, logAction);

            var thrownException = new TaskCanceledException();
            policy.Invoking(p => p.Execute(() => throw thrownException)).ShouldThrow<TaskCanceledException>();

            invokedLogger.Should().Be(expectedLogger);
            exceptionInvokedFor.Should().Be(thrownException);
            resultInvokedFor.Should().Be(default(HttpStatusCode));
        }

        [Fact] public void PolicyCallsTheLoggerIfPolicyHandlesResult()
        {
            ILogger expectedLogger = new StubLogger();
            Func<Context, ILogger> loggerProvider = _ => expectedLogger;

            ILogger invokedLogger = null;
            Exception exceptionInvokedFor = null;
            HttpStatusCode? resultInvokedFor = null;
            Action<ILogger, Context, DelegateResult<HttpStatusCode>> logAction = (logger, context, outcome) =>
            {
                invokedLogger = logger;
                exceptionInvokedFor = outcome.Exception;
                resultInvokedFor = outcome.Result;
            };

            LoggingPolicy<HttpStatusCode> policy = Policy<HttpStatusCode>
                .Handle<TaskCanceledException>()
                .OrResult(r => r != HttpStatusCode.OK)
                .Log(loggerProvider, logAction);

            var returnedResult = HttpStatusCode.InternalServerError;
            policy.Execute(() => returnedResult);

            invokedLogger.Should().Be(expectedLogger);
            exceptionInvokedFor.Should().BeNull();
            resultInvokedFor.Should().Be(returnedResult);
        }

        [Fact]
        public void PolicyDoesNotCallTheLoggerIfPolicyDoesNotHandleException()
        {
            ILogger expectedLogger = new StubLogger();
            Func<Context, ILogger> loggerProvider = _ => expectedLogger;

            ILogger invokedLogger = null;
            Exception exceptionInvokedFor = null;
            HttpStatusCode? resultInvokedFor = null;
            Action<ILogger, Context, DelegateResult<HttpStatusCode>> logAction = (logger, context, outcome) =>
            {
                invokedLogger = logger;
                exceptionInvokedFor = outcome.Exception;
                resultInvokedFor = outcome.Result;
            };

            LoggingPolicy<HttpStatusCode> policy = Policy<HttpStatusCode>
                .Handle<TaskCanceledException>()
                .OrResult(r => r != HttpStatusCode.OK)
                .Log(loggerProvider, logAction);

            var thrownException = new InvalidOperationException();
            policy.Invoking(p => p.Execute(() => throw thrownException)).ShouldThrow<InvalidOperationException>();

            invokedLogger.Should().BeNull();
            exceptionInvokedFor.Should().BeNull();
            resultInvokedFor.Should().BeNull();
        }

        [Fact]
        public void PolicyDoesNotCallTheLoggerIfSuccessfulExecution()
        {
            ILogger expectedLogger = new StubLogger();
            Func<Context, ILogger> loggerProvider = _ => expectedLogger;

            ILogger invokedLogger = null;
            Exception exceptionInvokedFor = null;
            HttpStatusCode? resultInvokedFor = null;
            Action<ILogger, Context, DelegateResult<HttpStatusCode>> logAction = (logger, context, outcome) =>
            {
                invokedLogger = logger;
                exceptionInvokedFor = outcome.Exception;
                resultInvokedFor = outcome.Result;
            };

            LoggingPolicy<HttpStatusCode> policy = Policy<HttpStatusCode>
                .Handle<TaskCanceledException>()
                .OrResult(r => r != HttpStatusCode.OK)
                .Log(loggerProvider, logAction);

            policy.Execute(() => HttpStatusCode.OK);

            invokedLogger.Should().BeNull();
            exceptionInvokedFor.Should().BeNull();
            resultInvokedFor.Should().BeNull();
        }

    }
}
