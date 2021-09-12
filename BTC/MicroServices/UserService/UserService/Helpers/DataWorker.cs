using UserService.Interfaces;
using System.Collections.Generic;

namespace UserService.Helpers
{
    public class DataWorker<T> : IDataWorker<T>
        where T : class
    {
        private readonly string _path;

        public DataWorker(string path)
        {
            _path = path;
        }

        public void CreateTable()
        {
            CsvHelper<T>.CreateWithHeader(_path);
        }

        public IEnumerable<T> ReadTable()
        {
            var data = CsvHelper<T>.Read(_path);
            return data;
        }

        public void WriteTable(T row)
        {
            CsvHelper<T>.Write(_path, row);
        }
    }
}
