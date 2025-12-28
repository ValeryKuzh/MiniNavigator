using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_UI.Service.ActionHandler
{
    public interface IObjectActionHandler
    {
        string CommandName { get; }
        bool CanExecute(ActionContext context);
        Task ExecuteAsync(ActionContext context);
    }
}
