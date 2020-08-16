using System.IO;
using Serilog.Events;
using Serilog.Parsing;

namespace TheDialgaTeam.Core.Logger.Formatter.Token
{
    public class ExceptionTokenFormatter : ITokenFormatter
    {
        private readonly PropertyToken _propertyToken;

        public ExceptionTokenFormatter(PropertyToken propertyToken)
        {
            _propertyToken = propertyToken;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (logEvent.Exception == null) return;

            AnsiEscapeCodeFormatter.Format(output, logEvent.Exception.ToString(), _propertyToken);
        }
    }
}