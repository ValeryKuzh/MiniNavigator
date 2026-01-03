using System;

namespace MiniNavigator_DB.Model
{
    /// <summary>
    /// Чанк файла
    /// </summary>
    public class ObjectFileChunk
    {
        public Guid ID { get; set; }

        public Guid ObjectFileID { get; set; } 
        public ObjectFile ObjectFile { get; set; } 

        public long NumberInSequence { get; set; }
        public byte[] Data { get; set; }
    }
}
