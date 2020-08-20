namespace YouTubePlays.Discord.Bot.Discord.Command
{
    public class CommandExecuteResult
    {
        private readonly string _message;
        private readonly bool _isSuccess;

        private CommandExecuteResult(string message, bool isSuccess)
        {
            _message = message;
            _isSuccess = isSuccess;
        }

        public static CommandExecuteResult FromSuccess(string message)
        {
            return new CommandExecuteResult(message, true);
        }

        public static CommandExecuteResult FromError(string message)
        {
            return new CommandExecuteResult(message, false);
        }

        public string BuildDiscordTextResponse()
        {
            return _isSuccess ? $":white_check_mark: {_message}" : $":x: {_message}";
        }
    }
}