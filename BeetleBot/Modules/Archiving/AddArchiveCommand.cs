using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeetleBot.Modules.Archiving
{
    public class AddArchiveCommand : ModuleBase<SocketCommandContext>
    {
        List<Archive> archiveList = Program.archiveList;
        [Command("addarchive")]
        public async Task AddArchiveAsync(ITextChannel sourceChan, ITextChannel destChan)
        {
            //==============================Delete Command Message=========================
            await Context.Message.DeleteAsync();
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
