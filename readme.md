# Sentry/OpenTelemetry Shutdown Bug

With Sentry & OpenTelemetry configured together I'm always getting the following error on shutdown of the process.

```
fail: Sentry.ISentryClient[0]
      Error while sending final client report (event ID: '(null)').
      System.InvalidCastException: Unable to cast object of type 'Sentry.Internal.NoOpTransaction' to type 'Sentry.TransactionTracer'.
         at Sentry.OpenTelemetry.SentrySpanProcessor.CreateRootSpan(Activity data)
         at Sentry.OpenTelemetry.SentrySpanProcessor.OnStart(Activity data)
         at OpenTelemetry.CompositeProcessor`1.OnStart(T data)
         at OpenTelemetry.Trace.TracerProviderSdk.<.ctor>b__12_1(Activity activity)
         at System.Diagnostics.SynchronizedList`1.EnumWithAction(Action`2 action, Object arg)
         at System.Diagnostics.Activity.Start()
         at System.Net.Http.DiagnosticsHandler.SendAsyncCore(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.RedirectHandler.SendAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.DecompressionHandler.SendAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at Sentry.Internal.Http.GzipBufferedRequestBodyHandler.SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
         at Sentry.Internal.Http.RetryAfterHandler.SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
         at System.Net.Http.HttpClient.<SendAsync>g__Core|83_0(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationTokenSource cts, Boolean disposeCts, CancellationTokenSource pendingRequestsCts, CancellationToken originalCancellationToken)
         at Sentry.Internal.Http.HttpTransport.SendEnvelopeAsync(Envelope envelope, CancellationToken cancellationToken)
         at Sentry.Internal.BackgroundWorker.SendFinalClientReportAsync(CancellationToken cancellationToken)
```

This was found in a number of projects after adding Sentry & OpenTelemetry support, but I've managed to cut it down to this minimal repo.

The `TracesSampleRate` appears to be key, if that's set to `1.0` and everything is sampled, the problem does not occur.

### To Reproduce

1. Clone project
1. Configure a DSN for a Sentry project in `program.cs`
1. Run the project
1. Make a test request to `http://localhost:5005`
1. Shut the process down with `CTRL+C`
1. Exception should appear
