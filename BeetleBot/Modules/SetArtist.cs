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
            if (Program.hasPermissions(Context.Guild.GetUser(Context.User.Id)))
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
                    if (user.Nickname == null && user.Username.Length <= 23)
                        x.Nickname = "[Artist] " + user.Username;
                    else if (user.Nickname == null && user.Username.Length > 23)
                        ReplyAsync(user.Username + "has too many characters in their name. Please change manually to less than 24 characters and try again.");
                    if (user.Nickname != null)
                    {
                        if (user.Nickname.Length <= 23)
                            x.Nickname = "[Artist] " + user.Nickname;
                        else
                            ReplyAsync(user.Nickname + "has too many characters in their name. Please change manually to less than 24 characters and try again.");
                    }
                });
            }
            else
                await ReplyAsync(Context.User.Mention + ", you do not have permission to change nicknames.");
        }
    }
}
