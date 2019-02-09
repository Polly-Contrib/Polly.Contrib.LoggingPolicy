using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Polly.Contrib.LoggingPolicy.Specs
{
    public class AsyncLoggingPolicySpecs
    {
        [Fact]
        public void PolicyCallsTheLoggerIfPolicyHandlesException()
        {
            ILogger expectedLogger = new StubLogger();
            Func<Context, ILogger> loggerProvider = _ => expectedLogger;

            ILogger invokedLogger = null;
            Exception exceptionInvokedFor = null;
            Action<ILogger, Context, Exception> logAction = (logger, context, exception) =>
            {
                invokedLogger = logger;
                exceptionInvokedFor = exception;
            };

            AsyncLoggingPolicy policy = Policy.Handle<TimeoutException>().AsyncLog(loggerProvider, logAction);

            var thrownException = new TimeoutException();
            policy.Awaiting(p => p.ExecuteAsync(() => throw thrownException)).ShouldThrow<TimeoutException>();

            invokedLogger.Should().Be(expectedLogger);
            exceptionInvokedFor.Should().Be(thrownException);
        }

        [Fact]
        public void PolicyDoesNotCallTheLoggerIfPolicyDoesNotHandleException()
        {
            ILogger expectedLogger = new StubLogger();
            Func<Context, ILogger> loggerProvider = _ => expectedLogger;

            ILogger invokedLogger = null;
            Exception exceptionInvokedFor = null;
            Action<ILogger, Context, Exception> logAction = (logger, context, exception) =>
            {
                invokedLogger = logger;
                exceptionInvokedFor = exception;
            };

            AsyncLoggingPolicy policy = Policy.Handle<TimeoutException>().AsyncLog(loggerProvider, logAction);

            var thrownException = new InvalidOperationException();
            policy.Awaiting(p => p.ExecuteAsync(() => throw thrownException)).ShouldThrow<InvalidOperationException>();

            invokedLogger.Should().BeNull();
            exceptionInvokedFor.Should().BeNull();
        }

        [Fact]
        public void PolicyDoesNotCallTheLoggerIfSuccessfulExecution()
        {
            ILogger expectedLogger = new StubLogger();
            Func<Context, ILogger> loggerProvider = _ => expectedLogger;

            ILogger invokedLogger = null;
            Exception exceptionInvokedFor = null;
            Action<ILogger, Context, Exception> logAction = (logger, context, exception) =>
            {
                invokedLogger = logger;
                exceptionInvokedFor = exception;
            };

            AsyncLoggingPolicy policy = Policy.Handle<TimeoutException>().AsyncLog(loggerProvider, logAction);

            policy.ExecuteAsync(() => Task.CompletedTask);

            invokedLogger.Should().BeNull();
            exceptionInvokedFor.Should().BeNull();
        }

    }
}
