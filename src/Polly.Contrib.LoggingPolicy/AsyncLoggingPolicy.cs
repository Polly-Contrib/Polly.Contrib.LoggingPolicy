using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polly.Contrib.LoggingPolicy /* Use a namespace broadly describing the topic, eg Polly.Contrib.Logging, Polly.Contrib.RateLimiting */
{
    /// <summary>
    /// A Logging policy that can be applied to asynchronous delegates.
    /// </summary>
    public class AsyncLoggingPolicy : AsyncPolicy, ILoggingPolicy
    {
        private readonly Func<Context, ILogger> _loggerProvider;
        private readonly Action<ILogger, Context, Exception> _logAction;

        internal AsyncLoggingPolicy(
            PolicyBuilder policyBuilder,
            Func<Context, ILogger> loggerProvider,
            Action<ILogger, Context, Exception> logAction)
            : base(policyBuilder)
        {
            _loggerProvider = loggerProvider ?? throw new NullReferenceException(nameof(loggerProvider));
            _logAction = logAction ?? throw new NullReferenceException(nameof(logAction));
        }

        /// <inheritdoc/>
        protected override Task<TResult> ImplementationAsync<TResult>(Func<Context, CancellationToken, Task<TResult>> action, Context context, CancellationToken cancellationToken,
            bool continueOnCapturedContext)
        {
            return AsyncLoggingEngine.ImplementationAsync(
                action,
                context,
                cancellationToken,
                continueOnCapturedContext,
                ExceptionPredicates,
                ResultPredicates<TResult>.None,
                _loggerProvider,
                (logger, ctx, delegateResult) => _logAction(logger, ctx, delegateResult.Exception)
                );
        }
    }
}