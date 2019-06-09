using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeetleBot.Modules
{
    public class Pic : ModuleBase<SocketCommandContext>
    {
        [Command("pic")]
        public async Task PicAsync()
        {

            
            IReadOnlyCollection<Discord.Rest.RestMessage> x = await Context.Channel.GetPinnedMessagesAsync();
            await ReplyAsync("Number of pinned messages in " + Context.Channel.Name + " is " + x.Count.ToString());
            foreach (Discord.Rest.RestMessage msg in x)
            {
                if (msg.Attachments.Count > 0)
                {
                    foreach (Discord.Attachment at in msg.Attachments)
                    {
                        WebClient saveFile = new WebClient();
                        string fileName = Path.GetFileName(at.Url);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                        saveFile.DownloadFile(at.Url, filePath);
                        
                        await ReplyAsync(at.Filename);
                        //await ReplyAsync(at.Url);
                        await Context.Channel.SendFileAsync(filePath);
                    }
                }
            }
        }
    }
}
