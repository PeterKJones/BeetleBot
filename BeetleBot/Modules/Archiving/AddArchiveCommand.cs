using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleBot.Modules.Archiving
{
    public class AddArchiveCommand : ModuleBase<SocketCommandContext>
    {
        [Command("addarchive")]
        public async Task AddArchiveAsync(ITextChannel sourceChan, ITextChannel destChan)
        {
            Archive temp = new Archive(sourceChan.Id, destChan.Id);
            await ReplyAsync("Now archiving " + sourceChan.Name + " to " + destChan.Name);
        }
    }
}
