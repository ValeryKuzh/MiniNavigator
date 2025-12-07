using MiniNavigator_Services.DTO;
using MiniNavigator_UI.Mapper.Interface;
using MiniNavigator_UI.ViewModel;

namespace MiniNavigator_UI.Mapper
{
    public class ObjectMapper : IMapper<NavObjectViewModel, NavObjectDTO>
    {
        public NavObjectViewModel ToViewModel(NavObjectDTO entity)
        {
            throw new System.NotImplementedException();
        }

        public NavObjectDTO ToDTO(NavObjectViewModel dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
