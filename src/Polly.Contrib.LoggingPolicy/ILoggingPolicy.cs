namespace Polly.Contrib.LoggingPolicy /* Use a namespace broadly describing the topic, eg Polly.Contrib.Logging, Polly.Contrib.RateLimiting */
{
    /// <summary>
    /// Defines properties common to synchronous and asynchronous Logging policies.
    /// </summary>
    public interface ILoggingPolicy : IsPolicy
    {
        /* Define properties (if any) or methods (if any) you may want to expose on LoggingPolicy.

         - Perhaps the custom policy takes configuration properties which you want to expose.
         - Perhaps the custom policy exposes methods for manual control.

        ... but it is equally common to have none.
         */
    }

    /// <summary>
    /// Defines properties common to generic, synchronous and asynchronous Logging policies.
    /// </summary>
    public interface ILoggingPolicy<TResult> : ILoggingPolicy
    {
        /* Define properties (if any) or methods (if any) you may want to expose on LoggingPolicy<TResult>.

           Typically, ILoggingPolicyPolicy<TResult> : ILoggingPolicyPolicy, so you would only expose here any 
           extra properties/methods typed in <TResult> for TResult policies.
         */
    }
}
