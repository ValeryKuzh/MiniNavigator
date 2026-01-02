using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
