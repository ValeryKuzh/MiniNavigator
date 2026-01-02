using MiniNavigator_DB.Model;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Mapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Mapper
{
    public class ChunkMapper : IMapper<ChunkDto, ObjectFileChunk>
    {
        public ChunkDto ToDTO(ObjectFileChunk entity)
        {
            throw new NotImplementedException();
        }

        public ObjectFileChunk ToEntity(ChunkDto DTO)
        {
            throw new NotImplementedException();
        }
    }
}
