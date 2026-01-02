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
    public class AttributeTypeMapper : IMapper<ObjectAttributeDTO, ObjectTypeAttribute>
    {
        public ObjectAttributeDTO ToDTO(ObjectTypeAttribute entity)
        {
            throw new NotImplementedException();
        }

        public ObjectTypeAttribute ToEntity(ObjectAttributeDTO DTO)
        {
            return new ObjectTypeAttribute
            {
                IsRequired = DTO.IsRequired,
                IsVisible = DTO.IsVisible,
                IsTitle = DTO.IsTitle,
                Order = DTO.Index
            };
        }
    }
}
