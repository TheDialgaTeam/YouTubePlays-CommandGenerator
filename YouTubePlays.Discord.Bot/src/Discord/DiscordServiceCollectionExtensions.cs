using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using YouTubePlays.Discord.Bot.Configuration;
using YouTubePlays.Discord.Bot.Discord.Client;
using YouTubePlays.Discord.Bot.Discord.Client.Event;
using YouTubePlays.Discord.Bot.Discord.Command;

namespace YouTubePlays.Discord.Bot.Discord
{
    public static class DiscordServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordShardedAppContext(this IServiceCollection serviceCollection, DiscordSocketConfig? discordSocketConfig = null)
        {
            serviceCollection.AddSingleton(serviceProvider => new DiscordShardedClient(discordSocketConfig ?? new DiscordSocketConfig()));
            serviceCollection.AddSingleton<DiscordConfiguration>();
            serviceCollection.AddSingleton<IDiscordShardedAppContextEvent, DiscordShardedAppContextLogger>();
            serviceCollection.AddSingleton<IDiscordShardedAppContextEvent, DiscordShardedAppContextStatus>();
            serviceCollection.AddSingleton(serviceProvider =>
            {
                var commandService = new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false });
                commandService.AddTypeReader<IEmote>(new EmoteTypeReader());
                commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), serviceProvider).GetAwaiter().GetResult();
                return commandService;
            });
            serviceCollection.AddSingleton<IDiscordShardedAppContextEvent, DiscordShardedAppContextCommand>();
            serviceCollection.AddSingleton<DiscordShardedAppContext>();
            return serviceCollection;
        }
    }
}