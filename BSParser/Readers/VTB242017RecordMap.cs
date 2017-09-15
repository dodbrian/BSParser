using CsvHelper.Configuration;

namespace BSParser.Readers
{
    public class VTB242017RecordMap : CsvClassMap<VTB242017Record>
    {
        public VTB242017RecordMap()
        {
            Map(m => m.Type).Index(0);
            Map(m => m.Date).Index(1);
            Map(m => m.Number).Index(2);
            Map(m => m.OperationType).Index(3);
            Map(m => m.Sum).Index(4);
            Map(m => m.Currency).Index(5);
            Map(m => m.Description).Index(6);
            Map(m => m.Bik).Index(7);
            Map(m => m.Account).Index(8);
            Map(m => m.Contractor).Index(9);
            Map(m => m.Bank).Index(10);
            Map(m => m.Inn).Index(11);
        }
    }
}