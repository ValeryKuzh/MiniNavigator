using MiniNavigator_Services.Service.Interface;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniNavigator_UI.Service.ActionHandler.Handler
{
    /// <summary>
    /// Обработчик действия "Удалить"
    /// </summary>
    public class DeleteActionHandler : IObjectActionHandler
    {
        public string CommandName => "DELETE";

        public bool CanExecute(ActionContext context)
            => context.Row != null && context.Row.ObjectId != Guid.Empty;

        public async Task ExecuteAsync(ActionContext context)
        {
            var form = context.Services.GetService(typeof(NavigatorForm)) as NavigatorForm;
            var objectService = context.Services.GetService(typeof(IObjectService)) as IObjectService;

            if (form == null || objectService == null)
                return;

            var result = MessageBox.Show(
                "Вы уверены, что хотите удалить объект?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                await objectService.DeleteObjectAsync(context.Row.ObjectId);

                form.RemoveRow(context.Row);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка удаления",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
