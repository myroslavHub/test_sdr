using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace NetSdrConsole;

public class Extensions
{
    public static ILogger SetupConsoleLogger()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(options =>
            {
                options.FormatterName = ConsoleFormatterNames.Simple;
            });

            builder.AddSimpleConsole(options =>
            {
                options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                options.IncludeScopes = false;
                options.SingleLine = true;
            });
            builder.SetMinimumLevel(LogLevel.Information);
        });
        return loggerFactory.CreateLogger("NetSdrConsole");
    }
}
