using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BeetleBot.Modules.Archiving
{
    public class AddArchiveCommand : ModuleBase<SocketCommandContext>
    {
        public List<Archive> archiveList = new List<Archive>();
        public Archive archive;
        [Command("addarchive")]
        public async Task AddArchiveAsync(ITextChannel sourceChan, ITextChannel destChan)
        {
            archive = new Archive(sourceChan.Id, destChan.Id);
            //archiveList = LoadArchiveConfig();
            AddArchive();
            await ReplyAsync("Now archiving " + sourceChan.Name + " to " + destChan.Name);
        }

        public void AddArchive()
        {
            if (!archiveList.Contains(archive))
            {
                archiveList.Add(archive);
                XmlSerializer archiveXML = new XmlSerializer(typeof(List<Archive>));
                TextWriter tw = new StreamWriter(Directory.GetCurrentDirectory() + "\\config.xml");
                archiveXML.Serialize(tw, archiveList);
                tw.Close();
            }

        }

        public void RemoveArchive()
        {
            if (archiveList.Contains(archive))
            {
                archiveList.Remove(archive);
                XmlSerializer archiveXML = new XmlSerializer(typeof(List<Archive>));
                TextWriter tw = new StreamWriter(Directory.GetCurrentDirectory() + "\\config.xml");
                archiveXML.Serialize(tw, archiveList);
            }
        }

        private List<Archive> LoadArchiveConfig()
        {
            string dir = Directory.GetCurrentDirectory() + "\\config.xml";
            if (Directory.Exists(dir))
                using (var sr = new StreamReader(dir))
                {
                    XmlSerializer archiveXML = new XmlSerializer(typeof(List<Archive>));
                    return (List<Archive>)archiveXML.Deserialize(sr);
                }
            return new List<Archive>();
        }
    }
}
