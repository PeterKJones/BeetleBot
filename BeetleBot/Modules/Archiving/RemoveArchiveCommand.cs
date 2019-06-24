using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeetleBot.Modules.Archiving
{
    public class RemoveArchiveCommand : ModuleBase<SocketCommandContext>
    {
        List<Archive> archiveList = Program.archiveList;
        [Command("removearchive")]
        public async Task RemoveArchiveAsync(ITextChannel sourceChan, ITextChannel destChan)
        {
            //==============================Delete Command Message=========================
            await Context.Message.DeleteAsync();
            //=============================================================================
            foreach (Archive a in Program.archiveList)
            {
                if (a.sourceName == sourceChan.Name && a.destName == destChan.Name)
                    switch (a.RemoveArchive())
                    {
                        case 0:
                            await ReplyAsync("Removed archive connection between " + sourceChan.Name + " and " + destChan.Name);
                            break;
                        case 1:
                            await ReplyAsync(sourceChan.Name + " and " + destChan.Name + " are not linked by an archive command.");
                            break;
                    }
            }
        }


    }
}
