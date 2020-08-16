using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Parsing;

namespace TheDialgaTeam.Core.Logger.Formatter.Token
{
    public class PropertiesTokenFormatter : ITokenFormatter
    {
        private readonly PropertyToken _propertyToken;
        private readonly MessageTemplate _messageTemplate;

        public PropertiesTokenFormatter(PropertyToken propertyToken, MessageTemplate messageTemplate)
        {
            _propertyToken = propertyToken;
            _messageTemplate = messageTemplate;
        }

        private static bool TemplateContainsPropertyName(MessageTemplate template, string propertyName)
        {
            var templateTokens = template.Tokens;

            foreach (var templateToken in templateTokens)
            {
                if (templateToken is PropertyToken propertyToken && propertyToken.PropertyName == propertyName)
                {
                    return true;
                }
            }

            return false;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            var included = logEvent.Properties
                .Where(a => !TemplateContainsPropertyName(logEvent.MessageTemplate, a.Key) && !TemplateContainsPropertyName(_messageTemplate, a.Key))
                .Select(a => new LogEventProperty(a.Key, a.Value));

            AnsiEscapeCodeFormatter.Format(output, string.Join(",", included.Select(a => $"{a.Name}: {a.Value}")), _propertyToken);
        }
    }
}