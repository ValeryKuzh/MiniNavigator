namespace MiniNavigator_Services.Mapper.Interface
{
    /// <summary>
    /// Интерфейс для маппинга классов 
    /// </summary>
    /// <typeparam name="DTO">DTO класс</typeparam>
    /// <typeparam name="Entity">Сущность из БД</typeparam>
    public interface IMapper<DTO, Entity> where DTO : class where Entity : class
    {
        DTO ToDto(Entity entity);
        Entity ToEntity(DTO dto);
    }
}
