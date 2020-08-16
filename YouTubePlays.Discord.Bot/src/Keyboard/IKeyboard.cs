namespace YouTubePlays.Discord.Bot.Keyboard
{
    public interface IKeyboard
    {
        string Name { get; }

        string ShortKey { get; }

        string GetChatCommand(string name, ChatBot chatBot);
    }
}