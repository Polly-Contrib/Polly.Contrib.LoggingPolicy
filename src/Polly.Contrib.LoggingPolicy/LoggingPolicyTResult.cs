using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Polly.Contrib.LoggingPolicy /* Use a namespace broadly describing the topic, eg Polly.Contrib.Logging, Polly.Contrib.RateLimiting */
{
    /// <summary>
    /// A Logging policy that can be applied to delegates returning a value of type <typeparamref name="TResult" />
    /// </summary>
    /// <typeparam name="TResult">The type of return values this policy will handle.</typeparam>
    public class LoggingPolicy<TResult> : Policy<TResult>, ILoggingPolicy<TResult>
    {
        private readonly Func<Context, ILogger> _loggerProvider;
        private readonly Action<ILogger, Context, DelegateResult<TResult>> _logAction;

        internal LoggingPolicy(
            PolicyBuilder<TResult> policyBuilder,
            Func<Context, ILogger> loggerProvider,
            Action<ILogger, Context, DelegateResult<TResult>> logAction)
            : base(policyBuilder)
        {
            _loggerProvider = loggerProvider ?? throw new NullReferenceException(nameof(loggerProvider));
            _logAction = logAction ?? throw new NullReferenceException(nameof(logAction));
        }

        /// <inheritdoc/>
        protected override TResult Implementation(Func<Context, CancellationToken, TResult> action, Context context, CancellationToken cancellationToken)
        {
            return LoggingEngine.Implementation(
                action, 
                context, 
                cancellationToken,
                ExceptionPredicates,
                ResultPredicates,
                _loggerProvider,
                _logAction
                );
        }
    }
}