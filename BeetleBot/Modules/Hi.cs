using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleBot.Modules
{
    public class Hi : ModuleBase<SocketCommandContext>
    {
        [Command("hi")]
        public async Task MelonAsync()
        {

            await ReplyAsync("Hello " + Context.User.Username);
        }
    }
}
