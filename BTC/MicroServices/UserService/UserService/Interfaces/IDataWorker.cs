using System.Collections.Generic;

namespace UserService.Interfaces
{
    public interface IDataWorker<T>
    {
        void CreateTable();
        IEnumerable<T> ReadTable();
        void WriteTable(T row);
    }
}