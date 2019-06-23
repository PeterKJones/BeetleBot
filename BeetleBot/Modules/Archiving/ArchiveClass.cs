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
        private List<Archive> archiveList;
        private string saveString;
        public Archive() { }

        public Archive(ulong sourceID, ulong destID, List<Archive> archiveList)
        {
            this.sourceID = sourceID;
            this.destID = destID;
            this.archiveList = archiveList;
            saveString = className + '|' + sourceName + '|' + sourceID + '|' + destName + '|' + destID;
        }

        public void AddArchive()
        {
            if (!archiveList.Contains(this))
            {
                archiveList.Add(this);
            }

        }

        public void RemoveArchive()
        {
            if (archiveList.Contains(this))
            {
                archiveList.Remove(this);
            }
        }

      
    }
}
