using MiniNavigator_Services.Service;
using MiniNavigator_Services.Service.Interface;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniNavigator_UI.Service.ActionHandler.Handler
{
    /// <summary>
    /// Обработчик действия "Скачать"
    /// </summary>
    public class DownloadActionHandler : IObjectActionHandler
    {
        public string CommandName => "DOWNLOAD";

        public bool CanExecute(ActionContext context)
            => context.Row != null && context.Row.ObjectId != Guid.Empty;

        public async Task ExecuteAsync(ActionContext context)
        {
            var fileService = context.Services.GetService(typeof(IFileService)) as IFileService;
            var objectTypeService = context.Services.GetService(typeof(IObjectTypeService)) as IObjectTypeService;

            try
            {
                var fileData = await fileService.DownloadFileAsync(context.Row.ObjectId);

                using (var dialog = new SaveFileDialog())
                {
                    var type = await objectTypeService.GetTypeByTypeObjectIDAsync(context.ObjectTypeId);
                    dialog.Filter = await fileService.GetOpenFileDialogFilterAsync(type.ID);

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {

                        using (var fs = new FileStream(
                            dialog.FileName,
                            FileMode.Create,
                            FileAccess.Write,
                            FileShare.None))
                        {
                            await fileData.CopyToAsync(fs);
                        }
                        MessageBox.Show("Файл скачан!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка скачивания",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
