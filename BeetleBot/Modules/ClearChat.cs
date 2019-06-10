using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleBot.Modules
{
    public class ClearChat : ModuleBase<SocketCommandContext>
    {
        [Command("clearchat")]
        public async Task ClearChatAsyn()
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
        }
    }
}
