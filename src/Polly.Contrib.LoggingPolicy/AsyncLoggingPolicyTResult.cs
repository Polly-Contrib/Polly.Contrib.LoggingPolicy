using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polly.Contrib.LoggingPolicy /* Use a namespace broadly describing the topic, eg Polly.Contrib.Logging, Polly.Contrib.RateLimiting */
{
    /// <summary>
    /// A Logging policy that can be applied to asynchronous delegates returning a value of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of return values this policy will handle.</typeparam>
    public class AsyncLoggingPolicy<TResult> : AsyncPolicy<TResult>, ILoggingPolicy<TResult>
    {
        private readonly Func<Context, ILogger> _loggerProvider;
        private readonly Action<ILogger, Context, DelegateResult<TResult>> _logAction;

        internal AsyncLoggingPolicy(
            PolicyBuilder<TResult> policyBuilder,
            Func<Context, ILogger> loggerProvider,
            Action<ILogger, Context, DelegateResult<TResult>> logAction)
            : base(policyBuilder)
        {
            _loggerProvider = loggerProvider ?? throw new NullReferenceException(nameof(loggerProvider));
            _logAction = logAction ?? throw new NullReferenceException(nameof(logAction));
        }

        /// <inheritdoc/>
        protected override Task<TResult> ImplementationAsync(Func<Context, CancellationToken, Task<TResult>> action, Context context, CancellationToken cancellationToken,
            bool continueOnCapturedContext)
        {
            return AsyncLoggingEngine.ImplementationAsync(
                action,
                context,
                cancellationToken,
                continueOnCapturedContext,
                ExceptionPredicates,
                ResultPredicates,
                _loggerProvider,
                _logAction
                );
        }
    }
}