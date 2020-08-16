using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Parsing;
using TheDialgaTeam.Core.Logger.Formatter.Token;

namespace TheDialgaTeam.Core.Logger.Formatter
{
    public class OutputTemplateTextFormatter : ITextFormatter
    {
        private readonly ITokenFormatter[] _tokenFormatters;

        public OutputTemplateTextFormatter(string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        {
            outputTemplate = outputTemplate ?? throw new ArgumentNullException(nameof(outputTemplate));

            var messageTemplate = new MessageTemplateParser().Parse(outputTemplate);
            var messageTemplateTokens = messageTemplate.Tokens;
            var tokenFormatters = new List<ITokenFormatter>();

            foreach (var messageTemplateToken in messageTemplateTokens)
            {
                switch (messageTemplateToken)
                {
                    case TextToken textToken:
                        tokenFormatters.Add(new TextTokenFormatter(textToken.Text));
                        break;

                    case PropertyToken propertyToken:
                        switch (propertyToken.PropertyName)
                        {
                            case OutputProperties.LevelPropertyName:
                                tokenFormatters.Add(new LevelTokenFormatter(propertyToken));
                                break;

                            case OutputProperties.ExceptionPropertyName:
                                tokenFormatters.Add(new ExceptionTokenFormatter(propertyToken));
                                break;

                            case OutputProperties.MessagePropertyName:
                                tokenFormatters.Add(new MessageTokenFormatter(propertyToken));
                                break;

                            case OutputProperties.NewLinePropertyName:
                                tokenFormatters.Add(new NewLineTokenFormatter(propertyToken));
                                break;

                            case OutputProperties.PropertiesPropertyName:
                                tokenFormatters.Add(new PropertiesTokenFormatter(propertyToken, messageTemplate));
                                break;

                            case OutputProperties.TimestampPropertyName:
                                tokenFormatters.Add(new TimestampTokenFormatter(propertyToken));
                                break;

                            default:
                                tokenFormatters.Add(new PropertyTokenFormatter(propertyToken));
                                break;
                        }

                        break;
                }
            }

            _tokenFormatters = tokenFormatters.ToArray();
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            foreach (var tokenRenderer in _tokenFormatters)
            {
                tokenRenderer.Format(logEvent, output);
            }
        }
    }
}