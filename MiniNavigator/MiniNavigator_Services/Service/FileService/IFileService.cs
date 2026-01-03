using System;
using System.IO;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service
{
    public interface IFileService
    {
        Task<string> GetOpenFileDialogFilterAsync(Guid objectTypeId);
        Task<bool> UploadFileAsync(Guid fileID, string filePath);
        Task<Stream> DownloadFileAsync(Guid fileID);
    }
}
