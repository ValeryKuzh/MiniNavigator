using MiniNavigator_Services.DTO;
using MiniNavigator_UI.Mapper.Interface;
using MiniNavigator_UI.ViewModel;
using System;
using System.Collections.Generic;

namespace MiniNavigator_UI.Mapper
{
    public class ObjectMapper : IMapper<NavObjectViewModel, NavObjectDTO>
    {
        public NavObjectViewModel ToViewModel(NavObjectDTO entity)
        {
            return new NavObjectViewModel()
            {
                ID = entity.ID,
                ObjectTypeID = entity.ObjectTypeID,
                ObjectTitle = entity.Title,
                ParentID = entity.ParentID
            };
        }

        public NavObjectDTO ToDTO(NavObjectViewModel dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
