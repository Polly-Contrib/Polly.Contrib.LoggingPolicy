using System;
using Microsoft.Extensions.Logging;

namespace Polly.Contrib.LoggingPolicy
{
    /// <summary>
    /// Contains configuration syntax for the <see cref="AsyncLoggingPolicy"/>
    /// </summary>
    public static class AsyncLoggingPolicySyntax
    {
        /// <summary>
        /// Constructs a new instance of <see cref="AsyncLoggingPolicy"/>, configured to handle the exceptions specified in the <paramref name="policyBuilder"/>.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="loggerProvider">A func returning a logger to use.</param>
        /// <param name="logAction">A logging action.</param>
        /// <returns><see cref="AsyncLoggingPolicy"/></returns>
        public static AsyncLoggingPolicy AsyncLog(
            this PolicyBuilder policyBuilder,
            Func<Context, ILogger> loggerProvider,
            Action<ILogger, Context, Exception> logAction
        )
        {
            return new AsyncLoggingPolicy(
                policyBuilder,
                loggerProvider,
                logAction
            );
        }
    }
}
