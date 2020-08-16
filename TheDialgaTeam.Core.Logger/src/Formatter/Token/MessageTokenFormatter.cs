using System.IO;
using Serilog.Events;
using Serilog.Parsing;

namespace TheDialgaTeam.Core.Logger.Formatter.Token
{
    public class MessageTokenFormatter : ITokenFormatter
    {
        private readonly PropertyToken _propertyToken;

        public MessageTokenFormatter(PropertyToken propertyToken)
        {
            _propertyToken = propertyToken;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            var textFormatter = new OutputTemplateTextFormatter(logEvent.MessageTemplate.Text);
            textFormatter.Format(logEvent, output);
        }
    }
}