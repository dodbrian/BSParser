using System;
using BSParser.Adapters;
using BSParser.Data;
using BSParser.Readers;
using BSParser.Writers;
using BSParser.Parser;
using System.Text;

namespace BSParser
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No parameters specified");
                return;
            }

            var inFile = args[0];

            var checker = new SourceChecker();

            var reader = checker.Detect(inFile);

            /*
                        if (reader != null)
                        {
                            Adapter adapter = new Adapter(reader);
                            adapter.Load();
                        }

                        ParseXML(inFile);
             */

            reader = new RosbankStatementReader(inFile);
            var adapter = new Adapter();
            adapter.Load(reader);
            var table = adapter.GetTransactions();
            /*
            foreach (StatementRow row in table)
            {
                Console.WriteLine("Дата = {0}, Сумма = {1}, Описание = {2}",
                    row.CreatedOn, row.Amount, row.Description);
            }
             */
            //Console.ReadLine();

            var parser = new Parser.Parser();
            parser.LoadRules(@"C:\Users\Denis\Documents\Visual Studio 2008\Projects\BSParser\BSForms\bin\Debug\Data\asdf.xml");

            parser.Parse(adapter.GetTransactions());

            var writer = new CSVWriter("outfile.csv");
            adapter.Export(writer);
        }
    }
}
