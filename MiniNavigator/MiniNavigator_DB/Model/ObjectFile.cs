using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniNavigator_DB.Model
{
    /// <summary>
    /// Файл в системе
    /// </summary>
    public class ObjectFile
    {
        public Guid ID { get; set; }
        public Guid Base_ID { get; set; }
        
        [ForeignKey("Base_ID")]
        public BaseObject Base { get; set; }

        public string FileExtension { get; set; }

        public ICollection<ObjectFileChunk> Chunks { get; set; }
    }
}