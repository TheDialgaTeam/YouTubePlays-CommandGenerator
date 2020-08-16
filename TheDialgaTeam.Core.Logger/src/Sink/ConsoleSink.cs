using System;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace TheDialgaTeam.Core.Logger.Sink
{
    public class ConsoleSink : ILogEventSink
    {
        private readonly LogEventLevel? _standardErrorFromLevel;
        private readonly ITextFormatter _formatter;
        private readonly object _syncRoot;

        public ConsoleSink(ITextFormatter formatter, LogEventLevel? standardErrorFromLevel, object syncRoot)
        {
            _formatter = formatter;
            _standardErrorFromLevel = standardErrorFromLevel;
            _syncRoot = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));
        }

        public void Emit(LogEvent logEvent)
        {
            var output = SelectOutputStream(logEvent.Level);

            lock (_syncRoot)
            {
                _formatter.Format(logEvent, output);
                output.Flush();
            }
        }

        private TextWriter SelectOutputStream(LogEventLevel logEventLevel)
        {
            var standardErrorFromLevel = _standardErrorFromLevel;

            if (standardErrorFromLevel == null) return Console.Out;

            return logEventLevel < standardErrorFromLevel ? Console.Out : Console.Error;
        }
    }
}