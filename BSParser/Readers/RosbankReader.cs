using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BSParser.Data;
using System.Xml;

namespace BSParser.Readers
{
    public class RosbankStatementReader : Reader
    {
        readonly XNamespace _ss = "urn:schemas-microsoft-com:office:spreadsheet";

        IEnumerable<XElement> _worksheets;
        IEnumerable<XElement> _rows;

        int _currentWorksheet;
        int _currentRow;

        string _currencyDecimalSeparator;

        public String FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                Init();
            }
        }

        bool MoveNext()
        {
            if (_currentRow < _rows.Count() - 1)
            {
                _currentRow++;
            }
            else
            {
                if (_currentWorksheet < _worksheets.Count() - 1)
                {
                    _currentWorksheet++;
                    _currentRow = 0;

                    // _rows = _worksheets.ElementAt(_currentWorksheet).Element(_ss + "Table").Elements(_ss + "Row").Skip(3);
                    var element = _worksheets.ElementAt(_currentWorksheet).Element(_ss + "Table");
                    if (element != null)
                        _rows = element.Elements(_ss + "Row").Skip(3);
                }
                else
                {
                    Eof = true;
                    return false;
                }
            }

            return true;
        }

        StatementRow ReadRow()
        {
            var outRow = new StatementRow();

            var row = _rows.ElementAt(_currentRow);

            // RefNum
            XElement element;

            try
            {
                element = row.Elements(_ss + "Cell").ElementAt(0).Element(_ss + "Data");
                if (element != null)
                    outRow.RefNum = element.Value;
            }
            catch
            {
                return null;
            }

            // DocNum
            try
            {
                element = row.Elements(_ss + "Cell").ElementAt(1).Element(_ss + "Data");
                if (element != null)
                    outRow.DocNum = element.Value;
            }
            catch
            {
                return null;
            }

            // Amount
            element = row.Elements(_ss + "Cell").ElementAt(3).Element(_ss + "Data");
            if (element != null)
            {
                var srcAmount = element.Value;
                if (_currencyDecimalSeparator != ".")
                    srcAmount = srcAmount.Replace('.', _currencyDecimalSeparator.ToCharArray()[0]);

                outRow.Amount = Decimal.Parse(srcAmount);
            }

            // Direction
            element = row.Elements(_ss + "Cell").ElementAt(4).Element(_ss + "Data");
            if (element != null)
            {
                outRow.Direction = element.Value == "Д" ? PaymentDirection.Debit : PaymentDirection.Credit;
            }

            // Description
            element = row.Elements(_ss + "Cell").ElementAt(5).Element(_ss + "Data");
            if (element != null)
                outRow.Description = element.Value;

            // RegisteredOn
            String srcRegisteredOn = null;
            try
            {
                element = row.Elements(_ss + "Cell").ElementAt(6).Element(_ss + "Data");
                if (element != null)
                    srcRegisteredOn = element.Value;
            }
            catch
            {
                return null;
                //srcRegisteredOn = null;
            }
            if (srcRegisteredOn != null)
                outRow.RegisteredOn = DateTime.Parse(srcRegisteredOn);

            // CreatedOn
            String srcCreatedOn = null;
            try
            {
                element = row.Elements(_ss + "Cell").ElementAt(7).Element(_ss + "Data");
                if (element != null)
                    srcCreatedOn = element.Value;
            }
            catch
            {
                return null;
                //srcCreatedOn = null;
            }
            if (srcCreatedOn != null)
                outRow.CreatedOn = DateTime.Parse(srcCreatedOn);

            // PayerBIK
            element = row.Elements(_ss + "Cell").ElementAt(8).Element(_ss + "Data");
            if (element != null)
                outRow.PayerBIK = element.Value;

            // PayerINN
            element = row.Elements(_ss + "Cell").ElementAt(12).Element(_ss + "Data");
            if (element != null)
                outRow.PayerINN = element.Value;

            // PayerName
            element = row.Elements(_ss + "Cell").ElementAt(14).Element(_ss + "Data");
            if (element != null)
                outRow.PayerName = element.Value;

            // ReceiverINN
            element = row.Elements(_ss + "Cell").ElementAt(19).Element(_ss + "Data");
            if (element != null)
                outRow.ReceiverINN = element.Value;

            // ReceiverName
            element = row.Elements(_ss + "Cell").ElementAt(21).Element(_ss + "Data");
            if (element != null)
                outRow.ReceiverName = element.Value;

            return outRow;
        }

        override public StatementRow Read()
        {
            StatementRow row = null;

            while (!Eof && row == null)
            {
                row = ReadRow();
                MoveNext();
            }

            return row;
        }

        override public bool Test()
        {
            return Test(fileName);
        }

        override public bool Test(String fileName)
        {
            var rdr = XmlReader.Create(fileName);

            try
            {
                while (rdr.Read())
                {
                    if (rdr.NodeType != XmlNodeType.Element || rdr.Name != "Data") continue;
                    if (!rdr.ReadString().Contains("Выписка из лицевого счета")) continue;

                    this.fileName = fileName;
                    Init();
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                rdr.Close();
            }

            return false;
        }

        void Init()
        {
            Type = ReaderType.RosbankXML;

            _currencyDecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.
                NumberFormat.CurrencyDecimalSeparator;

            _currentRow = 0;
            _currentWorksheet = 0;
            Eof = true;

            var doc = XDocument.Load(fileName);

            _worksheets = from wb in doc.Elements(_ss + "Workbook").Elements()
                          where wb.Name.LocalName == "Worksheet"
                          select wb;

            // Skip the table header
            var element = _worksheets.First().Element(_ss + "Table");
            if (element != null)
                _rows = element.Elements(_ss + "Row").Skip(3);

            if (_rows.Any()) Eof = false;

        }

        public RosbankStatementReader()
        {
        }

        public RosbankStatementReader(String fileName)
            : base(fileName)
        {
            Init();
        }

        public override void Dispose()
        {
        }
    }
}
