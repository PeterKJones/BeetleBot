using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeetleBot.Modules
{
    public class ClearChat : ModuleBase<SocketCommandContext>
    {
        [Command("clearchat")]
        public async Task ClearChatAsync(int clearAmount)
        {
            //==============================Delete Command Message=========================
            await Context.Message.DeleteAsync();
            //=============================================================================
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(clearAmount).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
        }
    }
}
