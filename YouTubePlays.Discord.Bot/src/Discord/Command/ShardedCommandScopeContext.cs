using System;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace YouTubePlays.Discord.Bot.Discord.Command
{
    public class ShardedCommandScopeContext : ShardedCommandContext
    {
        private readonly IServiceScope _serviceScope;
        public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

        public ShardedCommandScopeContext(DiscordShardedClient client, SocketUserMessage msg, IServiceScope serviceScope) : base(client, msg)
        {
            _serviceScope = serviceScope;
        }
    }
}