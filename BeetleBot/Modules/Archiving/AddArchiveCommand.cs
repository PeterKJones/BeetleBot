using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BeetleBot.Modules.Archiving
{
    public class AddArchiveCommand : ModuleBase<SocketCommandContext>
    {
        List<Archive> archiveList = Program.archiveList;
        [Command("addarchive")]
        public async Task AddArchiveAsync(ITextChannel sourceChan, ITextChannel destChan)
        {
            Archive archive = new Archive(sourceChan.Id, destChan.Id, archiveList);
            archive.AddArchive(); //Using custom method rather than the list add because it checks for more logic.
            await ReplyAsync("Now archiving " + sourceChan.Name + " to " + destChan.Name);
        }


    }
}
