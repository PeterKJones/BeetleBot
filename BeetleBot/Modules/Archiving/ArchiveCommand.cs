﻿using BeetleBot.Modules.Archiving;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BeetleBot.Modules
{
    public class ArchiveCommand : ModuleBase<SocketCommandContext>
    {
        [Command("archive")]
        public async Task PicAsync()
        {
            //==============================Delete Command Message=========================
            await Context.Message.DeleteAsync();
            //=============================================================================
            IReadOnlyCollection<Discord.Rest.RestMessage> x = await Context.Channel.GetPinnedMessagesAsync();
            //await ReplyAsync("Number of pinned messages in " + Context.Channel.Name + " is " + x.Count.ToString());
            var cmdUser = Context.User as SocketGuildUser;
            var role = (cmdUser as IGuildUser).Guild.Roles.FirstOrDefault(curRole => curRole.Id == 587350112536100884);
            if (cmdUser.Roles.Contains(role))
                foreach (Archive archive in Program.archiveList)
                {
                    var sourceChan = Context.Client.GetChannel(archive.sourceID) as IMessageChannel;
                    var destChan = Context.Client.GetChannel(archive.destID) as IMessageChannel;

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

                                    //await destChan.SendMessageAsync(at.Filename);
                                    await destChan.SendFileAsync(filePath);
                                    //File.Delete(filePath);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                            await msg.DeleteAsync();
                        }

                        //for url links, not uploads.
                        if (msg.ToString().ToLower().EndsWith(".jpg") || msg.ToString().ToLower().EndsWith(".png") || msg.ToString().ToLower().EndsWith(".jpeg"))
                        {
                            await Task.Run(async () =>
                            {
                                var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                foreach (Match m in linkParser.Matches(msg.ToString()))
                                {
                                    string fileName = Path.GetFileName(m.Value);
                                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                                    using (WebClient client = new WebClient())
                                        client.DownloadFile(new Uri(m.Value), filePath);
                                    //destChan.SendMessageAsync(fileName);
                                    await destChan.SendMessageAsync(m.Value);
                                }
                                await msg.DeleteAsync();
                            });
                            
                        }
                    }
                }
            else
                await ReplyAsync(Context.User.Username + ", you do not have permission to use this command.");
        }

        private void SaveFile(string source, string dest)
        {
            WebClient saveFile = new WebClient();

            if (File.Exists(dest))
            {
                var count = 2;
                string fullFilePath = Path.GetDirectoryName(dest) + Path.GetFileNameWithoutExtension(dest) + '_' + count + Path.GetExtension(dest);
                while (File.Exists(fullFilePath))
                {
                    count++;
                    fullFilePath = Path.GetDirectoryName(dest) + Path.GetFileNameWithoutExtension(dest) + '_' + count + Path.GetExtension(dest);
                }
                saveFile.DownloadFile(source, fullFilePath);
            }
            else
                saveFile.DownloadFile(source, dest);
        }
    }
}
