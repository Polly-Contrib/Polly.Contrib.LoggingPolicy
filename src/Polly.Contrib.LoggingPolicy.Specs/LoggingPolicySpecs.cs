using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Polly.Contrib.LoggingPolicy.Specs
{
    public class LoggingPolicySpecs
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

            LoggingPolicy policy = Policy.Handle<TimeoutException>().Log(loggerProvider, logAction);

            var thrownException = new TimeoutException();
            policy.Invoking(p => p.Execute(() => throw thrownException)).ShouldThrow<TimeoutException>();

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

            LoggingPolicy policy = Policy.Handle<TimeoutException>().Log(loggerProvider, logAction);

            var thrownException = new InvalidOperationException();
            policy.Invoking(p => p.Execute(() => throw thrownException)).ShouldThrow<InvalidOperationException>();

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

            LoggingPolicy policy = Policy.Handle<TimeoutException>().Log(loggerProvider, logAction);

            policy.Execute(() => { });

            invokedLogger.Should().BeNull();
            exceptionInvokedFor.Should().BeNull();
        }
    }
}