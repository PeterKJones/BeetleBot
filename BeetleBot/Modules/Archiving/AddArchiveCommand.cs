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
            //==============================Delete Command Message=========================
            IMessage cmdMsg = await Context.Channel.GetMessageAsync(Context.Message.Id);
            await cmdMsg.DeleteAsync();
            //=============================================================================
            Archive archive = new Archive(sourceChan.Name,sourceChan.Id, destChan.Name, destChan.Id);
            int added = archive.AddArchive(); //Using custom method rather than the list add because it checks for more logic.
            switch (added)
            {
                case 0:
                    await ReplyAsync("Now archiving " + sourceChan.Name + " to " + destChan.Name);
                    archive.SaveArchive();
                    break;
                case 1:
                    await ReplyAsync("Archive rule already established for " + sourceChan.Name + " and " + destChan.Name);
                    break;
            }
        }


    }
}
