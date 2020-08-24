using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YouTubePlays.Discord.Bot.Configuration;
using YouTubePlays.Discord.Bot.Discord.Command;

namespace YouTubePlays.Discord.Bot.Discord.Client.Event
{
    public class DiscordShardedAppContextCommand : IDiscordShardedAppContextEvent
    {
        private readonly DiscordConfiguration _discordConfiguration;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public DiscordShardedAppContextCommand(DiscordConfiguration discordConfiguration, CommandService commandService, IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
        {
            _discordConfiguration = discordConfiguration;
            _commandService = commandService;
            _serviceProvider = serviceProvider;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public void Add(DiscordShardedAppContext discordShardedAppContext)
        {
            discordShardedAppContext.MessageReceived += DiscordShardedAppContextOnMessageReceived;
        }

        public void Remove(DiscordShardedAppContext discordShardedAppContext)
        {
            discordShardedAppContext.MessageReceived -= DiscordShardedAppContextOnMessageReceived;
        }

        private Task DiscordShardedAppContextOnMessageReceived(DiscordShardedClient arg1, SocketMessage arg2)
        {
            return Task.Factory.StartNew(async state =>
            {
                if (state is (DiscordConfiguration discordConfiguration, CommandService commandService, IServiceProvider serviceProvider, DiscordShardedClient discordShardedClient, SocketMessage socketMessage))
                {
                    if (socketMessage is SocketUserMessage socketUserMessage)
                    {
                        using var scope = serviceProvider.CreateScope();

                        var context = new ShardedCommandScopeContext(discordShardedClient, socketUserMessage, scope);
                        var argPos = 0;
                        var botPrefix = discordConfiguration.BotPrefix;

                        if (socketUserMessage.Channel is SocketDMChannel)
                        {
                            if (socketUserMessage.HasMentionPrefix(discordShardedClient.CurrentUser, ref argPos) ||
                                socketUserMessage.HasStringPrefix(botPrefix, ref argPos, StringComparison.OrdinalIgnoreCase))
                            {
                            }
                        }
                        else
                        {
                            if (!socketUserMessage.HasMentionPrefix(discordShardedClient.CurrentUser, ref argPos) &&
                                !socketUserMessage.HasStringPrefix(botPrefix, ref argPos, StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }

                        await commandService.ExecuteAsync(context, argPos, scope.ServiceProvider).ConfigureAwait(false);
                    }
                }
            }, (_discordConfiguration, _commandService, _serviceProvider, arg1, arg2), _hostApplicationLifetime.ApplicationStopping, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
        }
    }
}