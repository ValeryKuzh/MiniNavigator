using System;
using System.Collections.Generic;

namespace MiniNavigator_DB.Model
{
    public class ObjectFile
    {
        public Guid ID { get; set; }
        public BaseObject Base { get; set; }

        public string FileExtension { get; set; }

        public ICollection<ObjectFileChunk> Chunks { get; set; }
    }
}
