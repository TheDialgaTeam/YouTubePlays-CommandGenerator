using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using TheDialgaTeam.Core.Logger.Sink;

namespace TheDialgaTeam.Core.Logger
{
    public static class ConsoleLoggerConfigurationExtensions
    {
        private static readonly object DefaultSyncRoot = new object();

        public static LoggerConfiguration AnsiConsole(this LoggerSinkConfiguration sinkConfiguration, ITextFormatter formatter, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, LoggingLevelSwitch? levelSwitch = null, LogEventLevel standardErrorFromLevel = LogEventLevel.Error, object? syncRoot = null)
        {
            syncRoot ??= DefaultSyncRoot;
            return sinkConfiguration.Sink(new ConsoleSink(formatter, standardErrorFromLevel, syncRoot), restrictedToMinimumLevel, levelSwitch);
        }
    }
}