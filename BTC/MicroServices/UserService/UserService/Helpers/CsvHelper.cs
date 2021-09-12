using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace UserService.Helpers
{
    public static class CsvHelper<T>
        where T : class
    {
        private static CsvConfiguration _configuration;

        private static object locker = new object();

        private static readonly string delimiter = ",";

        static CsvHelper()
        {
            _configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
                Encoding = System.Text.Encoding.Unicode,
                HasHeaderRecord = true
            };
        }

        public static IEnumerable<T> Read(string path)
        {
            IEnumerable<T> entries = null;

            lock (locker)
            {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, _configuration))
                {
                    entries = csv.GetRecords<T>();
                    return entries.ToList();
                }
            }
        }

        public static void Write(string path, T entity)
        {
            lock (locker)
            {
                using (var stream = File.Open(path, FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, _configuration))
                {
                    csv.WriteRecord(entity);
                    writer.Write("\n");
                }
            }
        }

        public static void CreateWithHeader(string path)
        {
            lock (locker)
            {
                using (var stream = File.Create(path))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<T>();
                    writer.Write("\n");
                }
            }
        }
    }
}
