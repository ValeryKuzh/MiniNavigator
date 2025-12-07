namespace MiniNavigator_UI.Mapper.Interface
{
    /// <summary>
    /// Интерфейс для маппинга классов ViewModel и DTO
    /// </summary>
    /// <typeparam name="ViewModel">ViewModel класс</typeparam>
    /// <typeparam name="DTO">DTO класс</typeparam>
    public interface IMapper<ViewModel, DTO> where ViewModel : class where DTO : class
    {
        ViewModel ToViewModel(DTO entity);
        DTO ToDTO(ViewModel dto);
    }
}
