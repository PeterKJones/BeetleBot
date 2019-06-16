using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace BeetleBot.Modules.Archiving
{
    [Serializable]
    public class Archive
    {
        private ulong sourceID { get; set; }
        private ulong destID { get; set; }

        public Archive() { }

        public Archive(ulong sourceID, ulong destID)
        {
            this.sourceID = sourceID;
            this.destID = destID;
        }
    }
}
