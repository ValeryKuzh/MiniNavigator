using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.DTO
{
    /// <summary>
    /// Класс для передачи Chunk
    /// </summary>
    public class ChunkDto
    {
        public Guid FileId { get; set; }
        public int NumberInSequence { get; set; }
        public byte[] Content { get; set; }
    }
}
