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
        public async Task ClearChatAsync()
        {
            //==============================Delete Command Message=========================
            IMessage cmdMsg = await Context.Channel.GetMessageAsync(Context.Message.Id);
            await cmdMsg.DeleteAsync();
            //=============================================================================
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
        }
    }
}
