using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeetleBot.Modules
{
    public class ArchiveCommand : ModuleBase<SocketCommandContext>
    {
        [Command("archive")]
        public async Task PicAsync()
        {

            
            IReadOnlyCollection<Discord.Rest.RestMessage> x = await Context.Channel.GetPinnedMessagesAsync();
            //await ReplyAsync("Number of pinned messages in " + Context.Channel.Name + " is " + x.Count.ToString());
            ulong id = 587398586870792205;
            var otherchannel = Context.Client.GetChannel(id) as IMessageChannel;
            var cmdUser = Context.User as SocketGuildUser;
            var role = (cmdUser as IGuildUser).Guild.Roles.FirstOrDefault(curRole => curRole.Name == "Admin");
            if (cmdUser.Roles.Contains(role))
                foreach (Discord.Rest.RestMessage msg in x)
                {
                    if (msg.Attachments.Count > 0) //for specifically attachments.
                    {
                        foreach (Attachment at in msg.Attachments)
                        {
                            try
                            {
                                string fileName = Path.GetFileName(at.Url);
                                string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                                SaveFile(at.Url, filePath);

                                await otherchannel.SendMessageAsync(at.Filename);
                                await otherchannel.SendFileAsync(filePath);
                                File.Delete(filePath);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        await msg.DeleteAsync();
                    }

                    if (msg.ToString().ToLower().EndsWith(".jpg") || msg.ToString().ToLower().EndsWith(".png") || msg.ToString().ToLower().EndsWith(".jpeg"))
                    {
                        //There could potentially be multiple images in one message. This needs to be parsed(most likely with regex)
                        string fileName = Path.GetFileName(msg.ToString());
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                        await otherchannel.SendMessageAsync(fileName);
                        await otherchannel.SendMessageAsync(msg.ToString());
                        await msg.DeleteAsync();
                    }
                }
            else
                await ReplyAsync(Context.User.Username + ", you do not have permission to use this command.");
        }

        private void SaveFile(string source, string dest)
        {
            WebClient saveFile = new WebClient();
            saveFile.DownloadFile(source, dest);
        }
    }
}
