using Microsoft.Extensions.Logging;

namespace Polly.Contrib.LoggingPolicy
{
    /// <summary>
    /// Defines extension methods on <see cref="Polly.Context"/> for registering and retrieving instances of <see cref="ILogger"/>.
    /// </summary>
    public static class ContextExtensions
    {
        private static readonly string LoggerKey = $"{nameof(LoggingPolicy)}.Logger";

        /// <summary>
        /// Registers a <see cref="ILogger"/> on the Polly execution <see cref="Context"/>, so that a policy from <see cref="Polly.Contrib.LoggingPolicy"/>
        /// can retrieve it using the <see cref="GetLogger"/> method.
        /// </summary>
        /// <param name="context">The Polly execution context.</param>
        /// <param name="logger">The logger to register on the execution context.</param>
        /// <returns>The Polly execution <see cref="Context"/>, for fluent chaining.</returns>
        public static Context WithLogger(this Context context, ILogger logger)
        {
            context[LoggerKey] = logger;
            return context;
        }

        /// <summary>
        /// Retrieves a <see cref="ILogger"/> previously registered on the Polly execution <see cref="Context"/>
        /// using the <see cref="WithLogger"/> method.
        /// </summary>
        /// <param name="context">The Polly execution context.</param>
        /// <returns>The <see cref="ILogger"/>; or null, if none was registered.</returns>
        public static ILogger GetLogger(this Context context)
        {
            if (context.TryGetValue(LoggerKey, out object logger))
            {
                return logger as ILogger;
            }

            return null;
        }
    }
}
