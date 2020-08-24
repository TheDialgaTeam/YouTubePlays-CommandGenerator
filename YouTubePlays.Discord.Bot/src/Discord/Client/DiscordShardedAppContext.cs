using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using YouTubePlays.Discord.Bot.Configuration;
using YouTubePlays.Discord.Bot.Discord.Client.Event;

namespace YouTubePlays.Discord.Bot.Discord.Client
{
    public class DiscordShardedAppContext
    {
        public event Func<DiscordShardedClient, LogMessage, Task>? Log;
        public event Func<DiscordShardedClient, DiscordSocketClient, Task>? ShardReady;
        public event Func<DiscordShardedClient, SocketMessage, Task>? MessageReceived;

        private readonly DiscordShardedClient _discordShardedClient;
        private readonly DiscordConfiguration _discordConfiguration;
        private readonly IEnumerable<IDiscordShardedAppContextEvent> _discordShardedAppContextEvents;

        public DiscordShardedAppContext(DiscordShardedClient discordShardedClient, DiscordConfiguration discordConfiguration, IEnumerable<IDiscordShardedAppContextEvent> discordShardedAppContextEvents)
        {
            _discordShardedClient = discordShardedClient;
            _discordConfiguration = discordConfiguration;
            _discordShardedAppContextEvents = discordShardedAppContextEvents;
        }

        public async Task StartAsync()
        {
            foreach (var discordShardedAppContextEvent in _discordShardedAppContextEvents)
            {
                discordShardedAppContextEvent.Add(this);
            }

            _discordShardedClient.Log += DiscordShardedClientOnLog;
            _discordShardedClient.ShardReady += DiscordShardedClientOnShardReady;
            _discordShardedClient.MessageReceived += DiscordShardedClientOnMessageReceived;

            await _discordShardedClient.LoginAsync(TokenType.Bot, _discordConfiguration.BotToken).ConfigureAwait(false);
            await _discordShardedClient.StartAsync().ConfigureAwait(false);
        }

        public Task StopAsync()
        {
            _discordShardedClient.Log -= DiscordShardedClientOnLog;
            _discordShardedClient.ShardReady -= DiscordShardedClientOnShardReady;
            _discordShardedClient.MessageReceived -= DiscordShardedClientOnMessageReceived;

            foreach (var discordShardedAppContextEvent in _discordShardedAppContextEvents)
            {
                discordShardedAppContextEvent.Remove(this);
            }

            return Task.CompletedTask;
        }

        private Task DiscordShardedClientOnLog(LogMessage arg)
        {
            return Log?.Invoke(_discordShardedClient, arg) ?? Task.CompletedTask;
        }

        private Task DiscordShardedClientOnShardReady(DiscordSocketClient arg)
        {
            return ShardReady?.Invoke(_discordShardedClient, arg) ?? Task.CompletedTask;
        }

        private Task DiscordShardedClientOnMessageReceived(SocketMessage arg)
        {
            return MessageReceived?.Invoke(_discordShardedClient, arg) ?? Task.CompletedTask;
        }
    }
}