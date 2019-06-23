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
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;
        public static List<Archive> archiveList = new List<Archive>();
        public static string configFile = Directory.GetCurrentDirectory() + "\\config.conf";

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
            
            
            //event subscriptions
            client.Log += Log;
            LoadArchiveConfig();
            await RegisterCommandsAsync();
            await client.LoginAsync(TokenType.Bot, botToken);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
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
