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
            bool canChange = false;
            SocketGuildUser user;
            using (IEnumerator<SocketUser> iterator = mentionedUsers.GetEnumerator())
            {
                iterator.MoveNext();
                user = Context.Guild.GetUser(iterator.Current.Id);
                //await ReplyAsync("Username of user attempting command: " + user.Username);
                //await ReplyAsync("Nickname of user attempting command: " + user.Nickname);
            }
            canChange = Context.Guild.GetUser(Context.User.Id).GuildPermissions.ManageNicknames;
            //await ReplyAsync("Permission check for ManageNicknames: " + canChange);
            //==============================Delete Command Message=========================
            await Context.Message.DeleteAsync();
            //=============================================================================
        
            if (user == null) return;
            if (canChange)
            {
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
