using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository.Interface;
using MiniNavigator_DB.Repository.ObjectFileRepository;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service
{
    /// <summary>
    /// Сервис для работы с файлами
    /// </summary>
    public class FileService : IFileService
    {
        private readonly IObjectTypeRepository _objectTypeRepository;
        private readonly IRepository<ObjectFile> _objectFileRepository;
        private readonly IRepository<ObjectFileChunk> _objectFileChunkRepository;

        private const int ChunkSize = 8192;

        public FileService
            (
                IObjectTypeRepository objectTypeRepository,
                IRepository<ObjectFile> objectFileRepository,
                IRepository<ObjectFileChunk> objectFileChunkRepository
            )
        {
            _objectTypeRepository = objectTypeRepository;
            _objectFileRepository = objectFileRepository;
            _objectFileChunkRepository = objectFileChunkRepository;
        }

        /// <summary>
        /// Получает фильтр для типа файла
        /// </summary>
        /// <param name="objectTypeID">ID объекта типа</param>
        /// <returns>Фильтр</returns>
        public async Task<string> GetOpenFileDialogFilterAsync(Guid objectTypeID)
        {
            if (objectTypeID == Guid.Empty) throw new ArgumentNullException(nameof(objectTypeID));
            var type = await _objectTypeRepository.GetByIdAsync(objectTypeID);

            if (type == null)
                return "Все файлы (*.*)|*.*";

            switch (type.Name)
            {
                case "PDF":
                    return "PDF файлы (*.pdf)|*.pdf";
                case "Excel":
                    return "Excel файлы (*.xlsx;*.xls)|*.xlsx;*.xls";
                default:
                    return "Все файлы (*.*)|*.*";
            }
        }

        /// <summary>
        /// Загружает файл в БД по чанкам
        /// </summary>
        /// <param name="fileObjectID">ID объекта файла</param>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Загружен файл или нет</returns>
        public async Task<bool> UploadFileAsync(Guid fileObjectID, string filePath)
        {

            if (fileObjectID == Guid.Empty) throw new ArgumentNullException(nameof(fileObjectID));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу пуст");

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var session = new FileUploadSession();

            try
            {
                var buffer = new byte[ChunkSize];
                int chunkNumber = 0;

                var fileEntity = CreateFileEntity(fileObjectID, filePath);
                
                var fileID = session.CreateFile(fileEntity);

                using (var fs = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: ChunkSize,
                    useAsync: true))
                {
                    int bytesRead;
                    while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        var chunkData = new byte[bytesRead];
                        Array.Copy(buffer, chunkData, bytesRead);

                        var chunkEntity = new ObjectFileChunk
                        {
                            ID = Guid.NewGuid(),
                            ObjectFileID = fileID,
                            NumberInSequence = chunkNumber++,
                            Data = chunkData
                        };

                        session.UploadChunk(chunkEntity);
                    }
                }

                session.Commit();
                return true;
            }
            catch
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// Создаёт запись о файле и возвращает его ID
        /// </summary>
        /// <returns>Файл</returns>
        private ObjectFile CreateFileEntity(Guid fileObjectID, string filePath)
        {
            if (fileObjectID == Guid.Empty) throw new ArgumentNullException(nameof(fileObjectID));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу пуст");

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            return new ObjectFile
            {
                FileExtension = Path.GetExtension(filePath),
                Base_ID = fileObjectID
            };
        }

        /// <summary>
        /// Собирает файл из БД
        /// </summary>
        /// <param name="fileID">ID файла</param>
        /// <returns>Поток для скачивания</returns>
        public async Task<Stream> DownloadFileAsync(Guid fileID)
        {
            if (fileID == Guid.Empty) throw new ArgumentNullException(nameof(fileID));

            var file = _objectFileRepository.Query().Where(x => x.Base_ID == fileID).First();
            if (file == null)
                throw new FileNotFoundException("Файл не найден");

            var chunks = _objectFileChunkRepository.Query()
                .Where(c => c.ObjectFileID == file.ID)
                .OrderBy(c => c.NumberInSequence)
                .ToList();

            if (chunks.Count == 0)
                throw new InvalidOperationException("Файл не содержит данных");

            var stream = new MemoryStream();

            foreach (var chunk in chunks)
            {
                await stream.WriteAsync(chunk.Data, 0, chunk.Data.Length);
            }

            stream.Position = 0;

            return stream;
        }
    }
}

