using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using BeetleBot.Modules.Archiving;

namespace BeetleBot
{
    public class Program
    {
        //TO DO:
        //Need to make sure directories exist if the bot is up for more than 24 hours because the days and even months can change if uptime is very long.
        //Change the order in which logged messages come in.
        //Make it so that the archive command will not allow a channel to be archived in more than one location.
        //Make sure the setartist role has the ability to set the mentioned user's nickname. If it's the owner, for example, that will not work.
        private static DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;
        //=================================Folders=========================================
        private static string contentFolder = Directory.GetCurrentDirectory() + "\\content\\Content_" + DateTime.Today.ToString("MMMM_yyyy") + "\\";
        //=================================Files===========================================
        public static string configFile = Directory.GetCurrentDirectory() + "\\config.conf";
        //=================================================================================

        public static List<Archive> archiveList = new List<Archive>();

        static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .BuildServiceProvider();

            string botToken = "NTg3MTY0NjI0MTUyMDM1Mzc1.XP1dBg.95bdsohUdxW9OiL65ffitq1ovPg";
            
            client.Log += Log;
            LoadArchiveConfig();
            await RegisterCommandsAsync();
            await client.LoginAsync(TokenType.Bot, botToken);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage arg) //Discord's log
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public static void SendLog(SocketUser commandUser, string operationName, IMessage rawMsg) //My Logging
        { 
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\log\\")) //create root log directory if it doesn't exist
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\log\\");

            string logFile = Directory.GetCurrentDirectory() + "\\log\\Log_" + DateTime.Today.ToString("MMddyyyy") + ".log"; //daily log file name

            if (rawMsg.Content.Length > 0) //if there is text.
            {
                string msg = DateTime.Now.ToString("[MM/dd/yyy hh:mm:sstt UTCz] ") + operationName + " Operation executed by " + commandUser.Username + " | [" + rawMsg.Timestamp.ToLocalTime() + "] " + rawMsg.Author + '(' + rawMsg.Author.Username + "): " + rawMsg;
                using (StreamWriter sw = File.AppendText(logFile))
                    sw.WriteLine(msg);
            }

            if (rawMsg.Attachments.Count > 0) //If there is an attachment
            {
                string msg = DateTime.Now.ToString("[MM/dd/yyy hh:mm:sstt UTCz] ") + operationName + " Operation executed by " + commandUser.Username + " | [" + rawMsg.Timestamp.ToLocalTime() + "] " + rawMsg.Author + '(' + rawMsg.Author.Username + ") had one or more attachments deleted.";
                using (StreamWriter sw = File.AppendText(logFile))
                    sw.WriteLine(msg);
            }

            if (rawMsg.MentionedUserIds.Count > 0) //If there is a mentioned user
            {
                foreach (ulong id in rawMsg.MentionedUserIds)
                {
                    SocketUser user = client.GetUser(id);
                    string msg = DateTime.Now.ToString("[MM/dd/yyy hh:mm:sstt UTCz] ") + operationName + " Operation executed by " + commandUser.Username + " | [" + rawMsg.Timestamp.ToLocalTime() + "] " + rawMsg.Author + '(' + rawMsg.Author.Username + ") mentioned " + user.Username;
                    using (StreamWriter sw = File.AppendText(logFile))
                        sw.WriteLine(msg);
                }
            }
        }

        public async Task RegisterCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            int argPos = 0;
            
            if (message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(client, message);
                var result = await commands.ExecuteAsync(context, argPos, services);

                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
        }

        private void LoadArchiveConfig()
        {
            if (File.Exists(configFile))
                using (StreamReader sr = File.OpenText(configFile))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine("Reading: " + s);
                        string[] entries = s.Split('|');
                        if (entries[0].Equals("Archive"))
                        {
                            Archive archive = new Archive(entries[1], ulong.Parse(entries[2]), entries[3], ulong.Parse(entries[4]));
                            archive.AddArchive();
                        }
                    }
                }
            else
            {
                using (StreamWriter sw = new StreamWriter(configFile))
                    sw.WriteLine("Beetlebot Configuration File - Date: " + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm:sstt"));
            }
        }
    }
}
