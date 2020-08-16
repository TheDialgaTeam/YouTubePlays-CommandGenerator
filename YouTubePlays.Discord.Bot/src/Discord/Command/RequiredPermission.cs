using System;
using System.Collections.Generic;
using System.Text;

namespace YouTubePlays.Discord.Bot.Discord.Command
{
    public enum RequiredPermission
    {
        GlobalDiscordAppOwner = 5,

        DiscordAppOwner = 4,

        GuildAdministrator = 3,

        GuildModerator = 2,

        ChannelModerator = 1,

        GuildMember = 0
    }
}
