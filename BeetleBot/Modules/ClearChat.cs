using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeetleBot.Modules
{
    public class ClearChat : ModuleBase<SocketCommandContext>
    {

        [Command("clearchat")]
        public async Task ClearChatAsync(int clearAmount)
        {
            if (Program.hasPermissions(Context.Guild.GetUser(Context.User.Id)))
            {
                if (clearAmount > 0 && clearAmount < 101)
                {
                    //==============================Delete Command Message=========================
                    await Context.Message.DeleteAsync();
                    //=============================================================================

                    var messages = await Context.Channel.GetMessagesAsync(clearAmount).FlattenAsync();
                    var reversed = messages.Reverse();
                    foreach (IMessage msg in reversed)
                        Program.SendLog(Context.User, "ClearChat", msg);
                    await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
                }
            }
            else
                await ReplyAsync(Context.User.Mention + ", you do not have permission to clear chat.");

        }
    }
}
