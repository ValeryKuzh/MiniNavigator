using MiniNavigator_Services.DTO;
using MiniNavigator_UI.Mapper.Interface;
using MiniNavigator_UI.ViewModel;

namespace MiniNavigator_UI.Mapper
{
    /// <summary>
    /// Маппер для NavObjectViewModel <=> NavObjectDTO
    /// </summary>
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
