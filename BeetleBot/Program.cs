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
        //Make it so that the archive command will not allow a channel to be archived in more than one location.
        //Make sure the setartist role has the ability to set the mentioned user's nickname. If it's the owner, for example, that will not work.
        //Make a move command for user content that was placed in the wrong channel.
        private static DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;
        //=================================Folders=========================================
        private static string contentFolder = Directory.GetCurrentDirectory() + "\\content\\Content_" + DateTime.Today.ToString("MMMM_yyyy") + "\\";
        //=================================Files===========================================
        public static string configFile = Directory.GetCurrentDirectory() + "\\config.conf";
        //=================================================================================

        public static List<Archive> archiveList = new List<Archive>();
        public static List<IRole> allowedRolesList = new List<IRole>();
        private ulong guildID = 587349550151368733;

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

            //=====Load Events========
            client.Log += Log;
            client.Ready += LoadConfig;
            //========================

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
        private Task LoadConfig()
        {
            if (File.Exists(configFile))
                using (StreamReader sr = File.OpenText(configFile))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine("Reading: " + s);
                        string[] entries = s.Split('|');
                        switch (entries[0].ToLower())
                        {
                            case "archive":
                                Archive archive = new Archive(entries[1], ulong.Parse(entries[2]), entries[3], ulong.Parse(entries[4]));
                                archive.AddArchive();
                                break;
                            case "allowedroles":
                                string[] roles = entries[1].Split(',');
                                var guildRoles = client.GetGuild(guildID).Roles;
                                foreach (string s2 in roles)
                                {
                                    foreach (SocketRole socketRole in guildRoles)
                                    {
                                        if (socketRole.Name.ToLower().Equals(s2.ToLower()))
                                        {
                                            allowedRolesList.Add(socketRole);
                                        }
                                    }
                                }
                                break;

                        }
                    }
                }
            else
            {
                using (StreamWriter sw = new StreamWriter(configFile))
                    sw.WriteLine("Beetlebot Configuration File - Date: " + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm:sstt"));
            }
            return Task.CompletedTask;
        }
        public static bool hasPermissions(SocketGuildUser socketUser)
        {
            var roles = socketUser.Roles;
            foreach (SocketRole socket in roles)
            {
                foreach (IRole role in allowedRolesList)
                {
                    if (socket.Name == role.Name)
                        return true;
                }
            }
            return false;
        }
    }
}
