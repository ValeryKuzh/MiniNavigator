using MiniNavigator_Services.Service;
using MiniNavigator_Services.Service.Interface;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniNavigator_UI.Service.ActionHandler.Handler
{
    /// <summary>
    /// Обработчик действия "Открыть"
    /// </summary>
    public class OpenActionHandler : IObjectActionHandler
    {
        public string CommandName => "OPEN";

        public bool CanExecute(ActionContext context)
            => context.Row != null && context.Row.ObjectId != Guid.Empty;


        public async Task ExecuteAsync(ActionContext context)
        {
            var form = context.Services.GetService(typeof(NavigatorForm)) as NavigatorForm;
            var fileService = context.Services.GetService(typeof(IFileService)) as IFileService;
            var objectTypeService = context.Services.GetService(typeof(IObjectTypeService)) as IObjectTypeService;

            if (form == null || fileService == null || objectTypeService == null)
                return;

            try
            {
                var fileStream = await fileService.DownloadFileAsync(context.Row.ObjectId);

                var type = await objectTypeService.GetTypeByTypeObjectIDAsync(context.ObjectTypeId);
                var filter = await fileService.GetOpenFileDialogFilterAsync(type.ID);
                var extension = ExtractExtensionFromFilter(filter);

                var tempFilePath = Path.Combine(
                    Path.GetTempPath(),
                    $"{Guid.NewGuid()}{extension}"
                );

                using (fileStream)
                {
                    using (var fs = new FileStream(
                        tempFilePath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.Read))
                    {
                        await fileStream.CopyToAsync(fs);
                    }
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = tempFilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка открытия файл",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string ExtractExtensionFromFilter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return ".tmp";

            var parts = filter.Split('|');
            if (parts.Length < 2)
                return ".tmp";

            var masks = parts[1].Split(';');

            return Path.GetExtension(masks[0]);
        }
    }
}

