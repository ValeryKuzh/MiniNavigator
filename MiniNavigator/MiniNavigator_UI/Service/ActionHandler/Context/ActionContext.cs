using MiniNavigator_UI.ViewModel;
using System;

namespace MiniNavigator_UI.Service.ActionHandler
{
    /// <summary>
    /// Объект контекста действия над объектом
    /// </summary>
    public class ActionContext
    {
        public string CommandName { get; set; }

        public DynamicObjectRow Row { get; set; }
        public Guid ObjectTypeId { get; set; }

        public IServiceProvider Services { get; set; }
    }
}
