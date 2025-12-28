using System.Threading.Tasks;

namespace MiniNavigator_UI.Service.ActionHandler
{
    public class EditActionHandler : IObjectActionHandler
    {
        public string CommandName => "EDIT";

        public bool CanExecute(ActionContext context)
            => context.Row != null;

        public Task ExecuteAsync(ActionContext context)
        {
            if (context.Services.GetService(typeof(NavigatorForm)) is NavigatorForm form)
            {
                form.BeginEditRow(context.Row);
            }

            return Task.CompletedTask;
        }
    }

}
