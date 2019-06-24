using System.IO;
using System.Linq;

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

        public int RemoveArchive()
        {
            if (isOnList())
            { 
                Program.archiveList.Remove(this);
                var tempFile = Path.GetTempFileName();
                var retainThis = File.ReadLines(Program.configFile).Where(l => l != this.saveString);
                File.WriteAllLines(tempFile, retainThis);
                File.Delete(Program.configFile);
                File.Move(tempFile, Program.configFile);
                return 0;
            }
            return 1;
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
