using System;
using System.IO;
using System.Text;
using BSParser.Data;

namespace BSParser.Writers
{
    /// <summary>
    /// Represents a CSV file writer
    /// </summary>
    public class CSVWriter : Writer
    {
        /// <summary>
        /// Output file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Character used to wrap a text column
        /// </summary>
        public char TextSeparator { get; set; }

        /// <summary>
        /// Character used for column separation
        /// </summary>
        public char ColumnSeparator { get; set; }

        /// <summary>
        /// Output text encoding
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Determines whether the table header should be included into the output file
        /// </summary>
        public bool IncludeHeader { get; set; }

        public CSVWriter()
        {
            IncludeHeader = true;
            Encoding = Encoding.Unicode;
            ColumnSeparator = '^';
            TextSeparator = '~';
        }

        public CSVWriter(String outFile)
            : this()
        {
            /*
                        IncludeHeader = true;
                        Encoding = Encoding.Unicode;
                        ColumnSeparator = '^';
                        TextSeparator = '~';
            */
            FileName = outFile;
        }

        public override bool Write(StatementTable data)
        {
            if (!String.IsNullOrEmpty(FileName))
            {
                var writer = new StreamWriter(FileName, false, Encoding);

                if (IncludeHeader)
                {
                    writer.WriteLine(GetHeader());
                }

                foreach (var row in data)
                {
                    var builder = new StringBuilder();

                    builder.Append(CSVTextColumn(row.Category));
                    builder.Append(CSVColumn(row.RefNum));
                    builder.Append(CSVColumn(row.DocNum));
                    builder.Append(CSVColumn(row.Amount.ToString()));
                    builder.Append(CSVColumn(row.Direction));
                    builder.Append(CSVColumn(row.RegisteredOn.ToShortDateString()));
                    builder.Append(CSVTextColumn(row.Description));
                    builder.Append(CSVColumn(row.PayerINN));
                    builder.Append(CSVTextColumn(row.PayerName));
                    builder.Append(CSVColumn(row.ReceiverINN));
                    builder.Append(CSVTextColumn(row.ReceiverName, true));

                    writer.WriteLine(builder.ToString());
                }

                writer.Close();

                return true;
            }

            return false;
        }

        protected String GetHeader()
        {
            var builder = new StringBuilder();

            builder.Append(CSVColumn("Category"));
            builder.Append(CSVColumn("RefNum"));
            builder.Append(CSVColumn("DocNum"));
            builder.Append(CSVColumn("Amount"));
            builder.Append(CSVColumn("Direction"));
            builder.Append(CSVColumn("CreatedOn"));
            builder.Append(CSVColumn("Description"));
            builder.Append(CSVColumn("PayerINN"));
            builder.Append(CSVColumn("PayerName"));
            builder.Append(CSVColumn("ReceiverINN"));
            builder.Append(CSVColumn("ReceiverName", true));

            return builder.ToString();
        }

        String CSVColumn(object obj)
        {
            return obj?.ToString() + ColumnSeparator;
        }

        String CSVColumn(object obj, bool last)
        {
            return last ? obj.ToString() : CSVColumn(obj);
        }

        String CSVTextColumn(object obj)
        {
            return TextSeparator + obj?.ToString() + TextSeparator + ColumnSeparator;
        }

        String CSVTextColumn(object obj, bool last)
        {
            if (last)
                return TextSeparator + obj?.ToString() + TextSeparator;
            return CSVTextColumn(obj);
        }
    }
}
