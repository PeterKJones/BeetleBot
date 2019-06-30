using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeetleBot.Modules
{
    public class ClearChat : ModuleBase<SocketCommandContext>
    {

        [Command("clearchat")]
        public async Task ClearChatAsync(int clearAmount)
        {
            if (clearAmount > 0 && clearAmount < 101)
            {
                //==============================Delete Command Message=========================
                await Context.Message.DeleteAsync();
                //=============================================================================


                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(clearAmount).FlattenAsync();
                foreach (IMessage msg in messages)
                {
                    Program.SendLog(Context.User,"ClearChat", msg);
                }
                await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
            }
            
        }
    }
}
