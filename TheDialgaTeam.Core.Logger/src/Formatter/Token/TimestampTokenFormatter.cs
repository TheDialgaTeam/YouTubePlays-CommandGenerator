using System.IO;
using Serilog.Events;
using Serilog.Parsing;

namespace TheDialgaTeam.Core.Logger.Formatter.Token
{
    public class TimestampTokenFormatter : ITokenFormatter
    {
        private readonly PropertyToken _propertyToken;

        public TimestampTokenFormatter(PropertyToken propertyToken)
        {
            _propertyToken = propertyToken;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            var propertyToken = _propertyToken;
            AnsiEscapeCodeFormatter.Format(output, logEvent.Timestamp.ToString(propertyToken.Format), propertyToken);
        }
    }
}