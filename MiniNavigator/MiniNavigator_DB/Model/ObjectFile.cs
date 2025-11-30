using System.Collections.Generic;

namespace MiniNavigator_DB.Model
{
    public class ObjectFile : BaseObject
    {
        public string FileExtension { get; set; }

        public ICollection<ObjectFileChunk> Chunks { get; set; }
    }
}
