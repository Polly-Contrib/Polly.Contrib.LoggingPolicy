using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Polly.Contrib.LoggingPolicy /* Use a namespace broadly describing the topic, eg Polly.Contrib.Logging, Polly.Contrib.RateLimiting */
{
    /// <summary>
    /// A Logging policy that can be applied to delegates.
    /// </summary>
    public class LoggingPolicy : Policy, ILoggingPolicy
    {
        private readonly Func<Context, ILogger> _loggerProvider;
        private readonly Action<ILogger, Context, Exception> _logAction;

        internal LoggingPolicy(
            PolicyBuilder policyBuilder,
            Func<Context, ILogger> loggerProvider,
            Action<ILogger, Context, Exception> logAction)
            : base(policyBuilder)
        {
            _loggerProvider = loggerProvider ?? throw new NullReferenceException(nameof(loggerProvider));
            _logAction = logAction ?? throw new NullReferenceException(nameof(logAction));
        }

        /// <inheritdoc/>
        protected override TResult Implementation<TResult>(Func<Context, CancellationToken, TResult> action, Context context, CancellationToken cancellationToken)
        {
            return LoggingEngine.Implementation(
                action, 
                context, 
                cancellationToken,
                ExceptionPredicates,
                ResultPredicates<TResult>.None,
                _loggerProvider,
                (logger, ctx, delegateResult) => _logAction(logger, ctx, delegateResult.Exception)
                );
        }
    }
}