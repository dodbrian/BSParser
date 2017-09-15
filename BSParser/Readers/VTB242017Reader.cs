using BSParser.Data;
using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace BSParser.Readers
{
    public class VTB242017Reader : Reader
    {
        private StreamReader _reader;
        private CsvReader _csv;

        private void Init()
        {
            Type = ReaderType.VTB242017;

            _reader = new StreamReader(fileName, Encoding.GetEncoding(1251));
            if (_reader == null) return;

            _csv = new CsvReader(_reader);

            _csv.Configuration.RegisterClassMap<VTB242017RecordMap>();
            _csv.Configuration.Delimiter = ";";

            // Read two auxiliary lines
            _csv.Read();
            _csv.Read();

            Eof = false;
        }

        public VTB242017Reader()
        {
            Eof = true;
        }

        public VTB242017Reader(string fileName) : base(fileName)
        {
            Eof = true;
            Init();
        }

        public override void Dispose()
        {
            if (_reader != null)
            {
                _reader.Close();
                _csv.Dispose();
            }
        }

        public override StatementRow Read()
        {
            if (!_csv.Read())
            {
                Eof = true;
                return null;
            }

            var record = _csv.GetRecord<VTB242017Record>();

            var amount = decimal.Parse(record.Sum, CultureInfo.InvariantCulture);

            var row = new StatementRow
            {
                RegisteredOn = Convert.ToDateTime(record.Date),
                DocNum = record.Number,
                Amount = Math.Abs(amount),
                Description = record.Description
            };

            if (amount > 0)
            {
                row.PayerName = record.Contractor;
                row.PayerINN = record.Inn;
                row.PayerBIK = record.Bik;
                row.Direction = PaymentDirection.Credit;
            }
            else
            {
                row.ReceiverName = record.Contractor;
                row.ReceiverINN = record.Inn;
                row.Direction = PaymentDirection.Debit;
            }

            return row;
        }

        public override bool Test()
        {
            return false;
        }

        public override bool Test(string fileName)
        {
            using (var testStream = new StreamReader(fileName, Encoding.GetEncoding(1251)))
            {
                var line = testStream.ReadLine();
                if (line != null)
                {
                    var testLine = line.Split(';');

                    if (testLine.Length == 12 && (testLine[0] == "Тип"))
                    {
                        this.fileName = fileName;
                        Init();
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
