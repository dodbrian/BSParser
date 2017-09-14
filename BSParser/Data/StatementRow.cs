using System;
using System.Collections.Generic;

namespace BSParser.Data
{
    public enum PaymentDirection
    {
        Debit = 0,
        Credit = 1
    }

    public enum StatementField
    {
        RefNum,
        DocNum,
        Amount,
        Direction,
        Description,
        RegisteredOn,
        CreatedOn,
        PayerBIK,
        PayerINN,
        PayerName,
        ReceiverINN,
        ReceiverName
    }

    public class StatementRow
    {
        public string RefNum { get; set; }

        public string DocNum { get; set; }

        public decimal Amount { get; set; }

        public PaymentDirection Direction { get; set; }

        public string Description { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public string PayerBIK { get; set; }

        public string PayerINN { get; set; }

        public string PayerName { get; set; }

        public string ReceiverINN { get; set; }

        public string ReceiverName { get; set; }

        public string Category { get; set; }
    }

    public class StatementTable : List<StatementRow>
    {
    }
}
