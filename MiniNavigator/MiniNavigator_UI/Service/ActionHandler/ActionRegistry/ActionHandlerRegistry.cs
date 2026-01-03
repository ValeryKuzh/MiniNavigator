using System.Collections.Generic;
using System.Linq;

namespace MiniNavigator_UI.Service.ActionHandler.Handler
{
    /// <summary>
    /// Регистратор обработчиков действий
    /// </summary>
    public class ActionHandlerRegistry
    {
        private readonly Dictionary<string, IObjectActionHandler> _handlers;

        public ActionHandlerRegistry(IEnumerable<IObjectActionHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.CommandName);
        }

        public IObjectActionHandler Resolve(string commandName)
            => _handlers[commandName];
    }
}
