using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleBot.Modules
{
    public class SetArtist : ModuleBase<SocketCommandContext>
    {
        [Command("setartist")]
        public async Task SetArtistAsync(string arg)
        {
            var mentionedUsers = Context.Message.MentionedUsers;
            SocketGuildUser user;
            using (IEnumerator<SocketUser> iterator = mentionedUsers.GetEnumerator())
            {
                iterator.MoveNext();
                user = Context.Guild.GetUser(iterator.Current.Id);
            }
            //==============================Delete Command Message=========================
            await Context.Message.DeleteAsync();
            //=============================================================================
        
            if (user == null) return;
            await user.ModifyAsync(x =>
            {
                x.Nickname = "[Artist] " + user.Nickname;
            });
        }
    }
}
