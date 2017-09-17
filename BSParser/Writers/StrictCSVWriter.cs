using BSParser.Data;
using CsvHelper;
using System.IO;
using System.Linq;
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
            if (!data.Any()) return false;

            var orderedData = data.Select(x => new
            {
                x.Category,
                x.RefNum,
                x.DocNum,
                x.Amount,
                x.Direction,
                x.RegisteredOn,
                x.Description,
                x.PayerINN,
                x.PayerName,
                x.ReceiverINN,
                x.ReceiverName
            });

            using (var sw = new StreamWriter(_fileName, false, Encoding.UTF8))
            {
                var csv = new CsvWriter(sw);
                csv.Configuration.Delimiter = ";";
                csv.WriteRecords(orderedData);
            }

            return true;
        }
    }
}
