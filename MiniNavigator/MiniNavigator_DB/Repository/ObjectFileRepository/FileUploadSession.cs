using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using System;
using System.Data.Entity;

namespace MiniNavigator_DB.Repository.ObjectFileRepository
{
    /// <summary>
    /// Сессия загрузки файла в БД
    /// </summary>
    public class FileUploadSession : IDisposable
    {
        private readonly MiniNavigatorDbContext _context;
        private readonly DbContextTransaction _transaction;

        private bool _committed = false;

        public FileUploadSession()
        {
            _context = new MiniNavigatorDbContext();
            _transaction = _context.Database.BeginTransaction();
        }

        /// <summary>
        /// Создаёт запись о файле и возвращает его Id
        /// </summary>
        public Guid CreateFile(ObjectFile objectFile)
        {
            objectFile.ID = Guid.NewGuid();

            _context.ObjectFiles.Add(objectFile);

            return objectFile.ID;
        }

        /// <summary>
        /// Добавляет один чанк к файлу
        /// </summary>
        public void UploadChunk(ObjectFileChunk objectFileChunk)
        {
            _context.ObjectFileChunks.Add(objectFileChunk);
        }

        /// <summary>
        /// Подтверждает запись всех файлов
        /// </summary>
        public void Commit()
        {
            if (_committed)
                return;

            _context.SaveChanges();
            _transaction.Commit();
            _committed = true;
        }

        /// <summary>
        /// Откат транзакции
        /// </summary>
        public void Rollback()
        {
            if (_committed)
                return;

            _transaction.Rollback();
            _committed = false;
        }

        public void Dispose()
        {
            if (!_committed)
                _transaction.Rollback();

            _transaction.Dispose();
            _context.Dispose();
        }
    }
}
