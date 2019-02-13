# Polly.Contrib.LoggingPolicy

This repo contains a custom [Polly](https://github.com/App-vNext/Polly) policy to log exceptions or handled results, then rethrow.

For more background on Polly see the [main Polly repo](https://github.com/App-vNext/Polly).

## Usage

#### Asynchronous executions

Define an `Action<ILogger, Context, DelegateResult<TResult>> logAction` to log exceptions or results.  The `Context` input parameter allows filtering based on Polly's in-built [execution metadata](https://github.com/App-vNext/Polly/wiki/Keys-And-Context-Data) or context you pass in to the execution.

    Action<ILogger, Context, DelegateResult<HttpResponseMessage>> logAction = (logger, context, outcome) => 
    {
        logger.LogError("The call resulted in outcome: {happened}", outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString());
    }

Define a `Func<Context, ILogger> loggerProvider` to select an `ILogger` based on `Context`.   This can use the extension method `Context.GetLogger()` defined on `Polly.Context`, if execution dispatch uses the `Context.WithLogger(ILogger)` overload, as demonstrated in the ConsoleApp example.

Configure the policy:

    var loggingPolicy = Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .OrResult((int)response.StatusCode >= 500 || response.StatusCode == HttpStatusCode.RequestTimeout)
        .AsyncLog(ctx => ctx.GetLogger(), logAction);

The policy would typically be combined with other policies in a PolicyWrap - see below.

#### Synchronous executions

Define a `logAction` and `loggerProvider` as above.

Configure the policy, for example:

    var loggingPolicy = Policy
        .Handle<Exception>()
        .Log(ctx => ctx.GetLogger(), logAction);

### Using LoggingPolicy in PolicyWrap

The `LoggingPolicy` can be used in any position in a [`PolicyWrap`](https://github.com/App-vNext/Polly/wiki/PolicyWrap).  

It will log the faults it is configured to handle, based on faults bubbled outwards from the next-inner level.  

+ A LoggingPolicy used **outermost** (configured first) in a PolicyWrap will log any eventual failure, after all policies of the PolicyWrap have exited.
+ A LoggingPolicy used **innermost** (configured last) in a PolicyWrap will log any failure from the underlying delegate executed through the PolicyWrap.  For instance, if the PolicyWrap includes retries, a logging policy configured inside (after, in HttpClientFactory sequence) the retry policy, will log any exception/fault thrown by each try.

The sole purpose of the policy is to log.  After logging, it bubbles faults further outwards to be handled by any policies further out or bubbled back to the caller.

### Blog post example

The policy is an example for the blog [Custom policies Part III: Authoring a reactive custom policy](LINK WHEN PUBLISHED).

The policy in this repo differs in small ways from the blog post, as this repo offers LoggingPolicy in all four combinations:

+ `LoggingPolicy` (synchronous non-generic)
+ `LoggingPolicy<TResult>` (synchronous generic)
+ `AsyncLoggingPolicy` (asynchronous non-generic)
+ `AsyncLoggingPolicy<TResult>` (asynchronous generic)

## Interested in developing your own custom policies?

See our blog series:

+ [Part I: Introducing custom Polly policies and the Polly.Contrib](http://www.thepollyproject.org/2019/02/13/introducing-custom-polly-policies-and-polly-contrib-custom-policies-part-i/)
+ [Part II: Authoring a non-reactive custom policy](http://www.thepollyproject.org/2019/02/13/authoring-a-proactive-polly-policy-custom-policies-part-ii/) (a policy which acts on all executions)
+ [Part III: Authoring a reactive custom policy](http://www.thepollyproject.org/2019/02/13/authoring-a-reactive-polly-policy-custom-policies-part-iii-2/) (a policy which react to faults)
+ [Part IV: Custom policies for all execution types](http://www.thepollyproject.org/2019/02/13/custom-policies-for-all-execution-types-custom-policies-part-iv/): sync and async, generic and non-generic

And see the templates for developing custom policies: [Polly.Contrib.CustomPolicyTemplates](https://github.com/Polly-Contrib/Polly.Contrib.CustomPolicyTemplates).
