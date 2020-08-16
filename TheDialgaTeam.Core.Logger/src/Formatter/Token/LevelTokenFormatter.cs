using System.IO;
using Serilog.Events;
using Serilog.Parsing;

namespace TheDialgaTeam.Core.Logger.Formatter.Token
{
    public class LevelTokenFormatter : ITokenFormatter
    {
        private readonly PropertyToken _propertyToken;

        public LevelTokenFormatter(PropertyToken propertyToken)
        {
            _propertyToken = propertyToken;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            AnsiEscapeCodeFormatter.Format(output, logEvent.Level.ToString(), _propertyToken);
        }
    }
}