using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Polly.Utilities;

namespace Polly.Contrib.LoggingPolicy
{
    internal static class LoggingEngine
    {
        internal static TResult Implementation<TResult>(
            Func<Context, CancellationToken, TResult> action,
            Context context,
            CancellationToken cancellationToken,
            ExceptionPredicates shouldHandleExceptionPredicates,
            ResultPredicates<TResult> shouldHandleResultPredicates,
            Func<Context, ILogger> loggerProvider,
            Action<ILogger, Context, DelegateResult<TResult>> logAction)
        {
            try
            {
                TResult result = action(context, cancellationToken);

                if (!shouldHandleResultPredicates.AnyMatch(result))
                {
                    return result; // Not an outcome the policy handles - just return it.
                }

                ILogger logger = loggerProvider(context);
                logAction(logger, context, new DelegateResult<TResult>(result));

                // The policy intentionally bubbles the result outwards after logging.
                return result;
            }
            catch (Exception exception)
            {
                Exception handledException = shouldHandleExceptionPredicates.FirstMatchOrDefault(exception);
                if (handledException == null)
                {
                    throw; // Not an exception the policy handles - propagate the exception.
                }

                ILogger logger = loggerProvider(context);
                logAction(logger, context, new DelegateResult<TResult>(exception));

                // The policy intentionally bubbles the exception outwards after logging.
                handledException.RethrowWithOriginalStackTraceIfDiffersFrom(exception);
                throw;
            }
        }
    }
}
