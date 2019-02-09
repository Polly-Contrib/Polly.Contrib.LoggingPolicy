using System;
using Microsoft.Extensions.Logging;

namespace Polly.Contrib.LoggingPolicy
{
    /// <summary>
    /// Contains configuration syntax for the <see cref="LoggingPolicy"/>
    /// </summary>
    public static class LoggingPolicySyntax
    {
        /// <summary>
        /// Constructs a new instance of <see cref="LoggingPolicy"/>, configured to handle the exceptions specified in the <paramref name="policyBuilder"/>.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="loggerProvider">A func returning a logger to use.</param>
        /// <param name="logAction">A logging action.</param>
        /// <returns><see cref="LoggingPolicy"/></returns>
        public static LoggingPolicy Log(
            this PolicyBuilder policyBuilder, 
            Func<Context, ILogger> loggerProvider,
            Action<ILogger, Context, Exception> logAction
        )
        {
            return new LoggingPolicy(
                policyBuilder,
                loggerProvider,
                logAction
            );
        }
    }
}
