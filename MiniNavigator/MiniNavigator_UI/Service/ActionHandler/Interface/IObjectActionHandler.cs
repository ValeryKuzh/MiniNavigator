using System.Threading.Tasks;

namespace MiniNavigator_UI.Service.ActionHandler
{
    /// <summary>
    /// Интерфейс обработчиков действий системы
    /// </summary>
    public interface IObjectActionHandler
    {
        string CommandName { get; }
        bool CanExecute(ActionContext context);
        Task ExecuteAsync(ActionContext context);
    }
}
