using System;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace TheDialgaTeam.Core.Logger.Sink
{
    public class ConsoleSink : ILogEventSink
    {
        private readonly LogEventLevel _standardErrorFromLevel;
        private readonly ITextFormatter _formatter;
        private readonly object _syncRoot;

        public ConsoleSink(ITextFormatter formatter, LogEventLevel standardErrorFromLevel, object syncRoot)
        {
            _formatter = formatter;
            _standardErrorFromLevel = standardErrorFromLevel;
            _syncRoot = syncRoot;
        }

        public void Emit(LogEvent logEvent)
        {
            var output = logEvent.Level < _standardErrorFromLevel ? Console.Out : Console.Error;

            lock (_syncRoot)
            {
                _formatter.Format(logEvent, output);
                output.Flush();
            }
        }
    }
}