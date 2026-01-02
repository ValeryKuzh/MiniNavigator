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
    public class FileMapper : IMapper<FileDto, ObjectFile>
    {
        public FileDto ToDTO(ObjectFile entity)
        {
            throw new NotImplementedException();
        }

        public ObjectFile ToEntity(FileDto DTO)
        {
            throw new NotImplementedException();
        }
    }
}
