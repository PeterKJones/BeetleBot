using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace BeetleBot.Modules.Archiving
{
    public class Archive
    {
        public string className = "Archive";
        public string sourceName;
        public string destName;
        public ulong sourceID;
        public ulong destID;
        private string saveString;
        public Archive() { }

        public Archive(string sourceName, ulong sourceID, string destName, ulong destID)
        {
            this.sourceName = sourceName;
            this.sourceID = sourceID;
            this.destName = destName;
            this.destID = destID;
            saveString = className + '|' + sourceName + '|' + sourceID + '|' + destName + '|' + destID;
        }

        public int AddArchive()
        {
            if (!isOnList())
            {
                Program.archiveList.Add(this);
                return 0;
            } 
            return 1;
        }

        public int SaveArchive()
        {
            if (isOnList())
            {
                using (StreamWriter sw = File.AppendText(Program.configFile))
                    sw.WriteLine(saveString);
                return 0;
            }
            return 1;
        }

        public void RemoveArchive()
        {
            if (Program.archiveList.Contains(this))
            {
                Program.archiveList.Remove(this);
            }
        }

        private bool isOnList()
        {
            foreach (Archive archive in Program.archiveList)
            {
                if (this.sourceID == archive.sourceID && this.destID == archive.destID)
                    return true;
            }
            return false;
        }

      
    }
}
