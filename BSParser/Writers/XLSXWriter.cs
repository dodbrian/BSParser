using BSParser.Data;
using ClosedXML.Excel;
using System.Linq;

namespace BSParser.Writers
{
    public class XLSXWriter : Writer
    {
        private string _filename;

        public XLSXWriter(string filename)
        {
            _filename = filename;
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

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Export");

            var props = orderedData.FirstOrDefault()?.GetType().GetProperties();
            if (props == null) return false;

            // Write header
            for (int i = 1; i <= props.Count(); i++)
                worksheet.Cell(1, i).Value = props[i - 1].Name;

            var row = 2;

            foreach (var item in orderedData)
            {
                worksheet.Cell(row, 1).Value = item.Category;
                worksheet.Cell(row, 2).Value = item.RefNum;
                worksheet.Cell(row, 3).Value = item.DocNum;
                worksheet.Cell(row, 4).Value = item.Amount;
                worksheet.Cell(row, 5).Value = item.Direction;
                worksheet.Cell(row, 6).Value = item.RegisteredOn;
                worksheet.Cell(row, 7).Value = item.Description;
                worksheet.Cell(row, 8).SetValue(item.PayerINN);
                worksheet.Cell(row, 9).Value = item.PayerName;
                worksheet.Cell(row, 10).SetValue(item.ReceiverINN);
                worksheet.Cell(row, 11).Value = item.ReceiverName;
                row++;
            }

            workbook.SaveAs(_filename);

            return true;
        }
    }
}
