using System.IO;
using Serilog.Events;
using Serilog.Parsing;

namespace TheDialgaTeam.Core.Logger.Formatter.Token
{
    public class PropertyTokenFormatter : ITokenFormatter
    {
        private readonly PropertyToken _propertyToken;

        public PropertyTokenFormatter(PropertyToken propertyToken)
        {
            _propertyToken = propertyToken;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            var propertyToken = _propertyToken;

            if (logEvent.Properties.TryGetValue(propertyToken.PropertyName, out var logEventPropertyValue))
            {
                var writer = new StringWriter();
                logEventPropertyValue.Render(writer, propertyToken.Format);
                AnsiEscapeCodeFormatter.Format(output, writer.ToString(), propertyToken);
            }
            else
            {
                AnsiEscapeCodeFormatter.Format(output, string.Empty, propertyToken);
            }
        }
    }
}