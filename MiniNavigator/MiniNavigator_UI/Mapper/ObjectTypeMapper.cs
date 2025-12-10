using MiniNavigator_Services.DTO;
using MiniNavigator_UI.Mapper.Interface;
using MiniNavigator_UI.ViewModel;

namespace MiniNavigator_UI.Mapper
{
    public class ObjectTypeMapper : IMapper<ObjectTypeViewModel, ObjectTypeDTO>
    {
        public ObjectTypeDTO ToDTO(ObjectTypeViewModel dto)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTypeViewModel ToViewModel(ObjectTypeDTO entity)
        {
            return new ObjectTypeViewModel
            {
                Name = entity.Title
            };
        }
    }
}
