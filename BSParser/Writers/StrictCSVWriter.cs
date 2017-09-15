using BSParser.Data;
using CsvHelper;
using System.IO;
using System.Text;

namespace BSParser.Writers
{
    public class StrictCSVWriter : Writer
    {
        private string _fileName;

        public StrictCSVWriter(string fileName)
        {
            _fileName = fileName;
        }

        public override bool Write(StatementTable data)
        {
            using (var sw = new StreamWriter(_fileName, false, Encoding.UTF8))
            {
                var csv = new CsvWriter(sw);
                csv.Configuration.Delimiter = ";";
                csv.WriteRecords(data);
            }

            return true;
        }
    }
}
