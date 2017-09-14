using System.Linq;
using BSParser.Readers;
using BSParser.Data;
using BSParser.Writers;

namespace BSParser.Adapters
{
    public class Adapter
    {
        protected StatementTable Transactions;

        public void Load(Reader reader)
        {
            if (Transactions == null)
                Transactions = new StatementTable();

            while (!reader.Eof)
            {
                var row = reader.Read();
                if (row != null)
                    Transactions.Add(row);
            }
        }

        public StatementTable GetTransactions()
        {
            return Transactions;
        }

        /// <summary>
        /// Exports all transactions using given Writer
        /// </summary>
        /// <param name="writer">Certan Writer object</param>
        /// <returns>Returns <c>true</c> if operation was successful</returns>
        public bool Export(Writer writer)
        {
            if (Transactions != null && Transactions.Any())
            {
                return writer.Write(Transactions);
            }

            return false;
        }
    }

}
