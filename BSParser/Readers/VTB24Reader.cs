using BSParser.Data;
using System;
using System.IO;
using System.Text;

namespace BSParser.Readers
{
    public class VTB24StatementReader : Reader
    {
        StreamReader _stream;
        String _currencyDecimalSeparator;

        public String FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                Init();
            }
        }

        override public StatementRow Read()
        {
            if (!_stream.EndOfStream)
            {
                var line = _stream.ReadLine();

                if (line != null)
                {
                    if (line == "\"")
                        return null;

                    var columns = line.Split(';');

                    if (columns.Length < 11)
                        return null;

                    var row = new StatementRow
                    {
                        RefNum = "",
                        RegisteredOn = Convert.ToDateTime(columns[1]),
                        DocNum = columns[2].Trim('"')
                    };

                    if (_currencyDecimalSeparator != ".")
                    {
                        columns[4] = columns[4].Replace(".", _currencyDecimalSeparator);
                    }

                    row.Amount = Convert.ToDecimal(columns[4]);

                    row.Description = StripQuotes(columns[6]);

                    if (row.Amount > 0)
                    {
                        row.PayerName = StripQuotes(columns[9]);
                        row.PayerINN = columns[10].Trim('"');
                        row.PayerBIK = columns[7].Trim('"');
                        row.ReceiverName = "";
                        row.ReceiverINN = "";
                        row.Direction = PaymentDirection.Credit;
                    }
                    else
                    {
                        row.ReceiverName = StripQuotes(columns[9]);
                        row.ReceiverINN = columns[10].Trim('"');
                        row.PayerBIK = "";
                        row.PayerName = "";
                        row.PayerINN = "";
                        row.Direction = PaymentDirection.Debit;
                    }

                    row.Amount = Math.Abs(row.Amount);

                    return row;
                }
            }
            Eof = true;

            return null;
        }

        static String StripQuotes(String line)
        {
            line = line.Replace("\"\"", "^");
            line = line.Trim('"');
            line = line.Replace('^', '"');

            return line;
        }

        override public bool Test()
        {
            return false;
        }

        override public bool Test(String fileName)
        {
            using (var testStream = new StreamReader(fileName, Encoding.GetEncoding(1251)))
            {
                var line = testStream.ReadLine();
                if (line != null)
                {
                    var testLine = line.Split(';');

                    if (testLine.Length == 11 && (testLine[0] == "“ËÔ"))
                    {
                        this.fileName = fileName;
                        Init();
                        return true;
                    }
                }

                return false;
            }
        }

        public VTB24StatementReader()
        {
            Eof = true;
        }

        public VTB24StatementReader(String fileName)
            : base(fileName)
        {
            Eof = true;
            Init();
        }

        void Init()
        {
            Type = ReaderType.VTB24;

            _currencyDecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.
                NumberFormat.CurrencyDecimalSeparator;

            _stream = new StreamReader(fileName, Encoding.GetEncoding("windows-1251"));

            if (_stream == null) return;
            // Read CSV header
            _stream.ReadLine();
            Eof = false;
        }

        public override void Dispose()
        {
            if (_stream != null)
            {
                _stream.Close();
            }
        }
    }
}
