using OpenTelemetry.Trace;
using Sentry.OpenTelemetry;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .UseDefaultServiceProvider(configure => configure.ValidateScopes = false)
    .ConfigureLogging(logging => logging.AddConsole())
    .UseSentry(options =>
    {
        options.Dsn = "";
        options.Debug = true;
        options.TracesSampleRate = 0.01;

        options.UseOpenTelemetry();
    });

builder.Services
    .AddOpenTelemetry()
        .WithTracing(tracing =>
        {
            tracing
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SentryShutdown"))
                .AddSentry()
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter();
        });

var app = builder.Build();
app.MapGet("/", () => "Hello World");
await app.RunAsync();