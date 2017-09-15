using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BSParser.Data;
using System.Text.RegularExpressions;

namespace BSParser.Parser
{
    /// <summary>
    /// Represents a conditional element of the Rule object
    /// </summary>
    public class Condition
    {
        public StatementField Field { get; set; }

        public Operation Operation { get; set; }

        public string TestValue { get; set; }

        public Condition()
        {
        }

        public Condition(StatementField field, Operation operation, String testValue)
        {
            Field = field;
            Operation = operation;
            TestValue = testValue;
        }

        public Condition(String field, String operation, String testValue)
        {
            switch (field)
            {
                case "RefNum":
                    Field = StatementField.RefNum;
                    break;
                case "DocNum":
                    Field = StatementField.DocNum;
                    break;
                case "Amount":
                    Field = StatementField.Amount;
                    break;
                case "Direction":
                    Field = StatementField.Direction;
                    break;
                case "Description":
                    Field = StatementField.Description;
                    break;
                case "RegisteredOn":
                    Field = StatementField.RegisteredOn;
                    break;
                case "CreatedOn":
                    Field = StatementField.CreatedOn;
                    break;
                case "PayerBIK":
                    Field = StatementField.PayerBIK;
                    break;
                case "PayerINN":
                    Field = StatementField.PayerINN;
                    break;
                case "PayerName":
                    Field = StatementField.PayerName;
                    break;
                case "ReceiverINN":
                    Field = StatementField.ReceiverINN;
                    break;
                case "ReceiverName":
                    Field = StatementField.ReceiverName;
                    break;
                default:
                    throw new Exception("Unknown field specified in rule's condition");
            }

            switch (operation)
            {
                case "Equal":
                    Operation = Operation.Equal;
                    break;
                case "NotEqual":
                    Operation = Operation.NotEqual;
                    break;
                case "Contains":
                    Operation = Operation.Contains;
                    break;
                case "NotContains":
                    Operation = Operation.NotContains;
                    break;
                case "GreaterThan":
                    Operation = Operation.GreaterThan;
                    break;
                case "LessThan":
                    Operation = Operation.LessThan;
                    break;
                case "RegEx":
                    Operation = Operation.RegEx;
                    break;
            }

            TestValue = testValue;
        }

        public bool Check(String valueToCheck)
        {
            if (valueToCheck == null) return false;

            switch (Operation)
            {
                case Operation.Contains:
                    if (valueToCheck.ToUpper().Contains(TestValue.ToUpper()))
                    {
                        return true;
                    }
                    break;
                case Operation.NotContains:
                    if (!valueToCheck.ToUpper().Contains(TestValue.ToUpper()))
                    {
                        return true;
                    }
                    break;
                case Operation.Equal:
                    if (valueToCheck == TestValue)
                    {
                        return true;
                    }
                    break;
                case Operation.NotEqual:
                    if (valueToCheck != TestValue)
                    {
                        return true;
                    }
                    break;
                case Operation.RegEx:
                    return Regex.IsMatch(valueToCheck, TestValue);
                default:
                    throw new Exception("Unknown operation in condition or operation is not implemented");
            }

            return false;
        }

        public bool Check(decimal valueToCheck)
        {
            switch (Operation)
            {
                case Operation.Contains:
                    if (valueToCheck.ToString().ToUpper().Contains(TestValue.ToUpper()))
                    {
                        return true;
                    }
                    break;
                case Operation.NotContains:
                    if (!valueToCheck.ToString().ToUpper().Contains(TestValue.ToUpper()))
                    {
                        return true;
                    }
                    break;
                case Operation.Equal:
                    if (valueToCheck == Convert.ToDecimal(TestValue))
                    {
                        return true;
                    }
                    break;
                case Operation.NotEqual:
                    if (valueToCheck != Convert.ToDecimal(TestValue))
                    {
                        return true;
                    }
                    break;
                case Operation.GreaterThan:
                    if (valueToCheck > Convert.ToDecimal(TestValue))
                    {
                        return true;
                    }
                    break;
                case Operation.LessThan:
                    if (valueToCheck < Convert.ToDecimal(TestValue))
                    {
                        return true;
                    }
                    break;
                case Operation.RegEx:
                    return Regex.IsMatch(valueToCheck.ToString(), TestValue);
                default:
                    throw new Exception("Unknown operation in condition or operation is not implemented");
            }

            return false;
        }

        public bool Check(DateTime valueToCheck)
        {
            switch (Operation)
            {
                case Operation.Contains:
                    if (valueToCheck.ToString().ToUpper().Contains(TestValue.ToUpper()))
                    {
                        return true;
                    }
                    break;
                case Operation.NotContains:
                    if (!valueToCheck.ToString().ToUpper().Contains(TestValue.ToUpper()))
                    {
                        return true;
                    }
                    break;
                case Operation.Equal:
                    if (valueToCheck == Convert.ToDateTime(TestValue))
                    {
                        return true;
                    }
                    break;
                case Operation.NotEqual:
                    if (valueToCheck != Convert.ToDateTime(TestValue))
                    {
                        return true;
                    }
                    break;
                case Operation.GreaterThan:
                    if (valueToCheck > Convert.ToDateTime(TestValue))
                    {
                        return true;
                    }
                    break;
                case Operation.LessThan:
                    if (valueToCheck < Convert.ToDateTime(TestValue))
                    {
                        return true;
                    }
                    break;
                case Operation.RegEx:
                    return Regex.IsMatch(valueToCheck.ToShortDateString(), TestValue);
                default:
                    throw new Exception("Unknown operation in condition or operation is not implemented");
            }

            return false;
        }

        public bool Check(PaymentDirection valueToCheck)
        {
            switch (Operation)
            {
                case Operation.Contains:
                    if (valueToCheck.ToString().ToUpper().Contains(TestValue.ToUpper()))
                    {
                        return true;
                    }
                    break;
                case Operation.NotContains:
                    if (!valueToCheck.ToString().ToUpper().Contains(TestValue.ToUpper()))
                    {
                        return true;
                    }
                    break;
                case Operation.Equal:
                    if (valueToCheck.ToString().ToUpper() == TestValue.ToUpper())
                    {
                        return true;
                    }
                    break;
                case Operation.NotEqual:
                    if (valueToCheck.ToString().ToUpper() != TestValue.ToUpper())
                    {
                        return true;
                    }
                    break;
                case Operation.RegEx:
                    return Regex.IsMatch(valueToCheck.ToString(), TestValue);
                default:
                    throw new Exception("Unknown operation in condition or operation is not implemented");
            }

            return false;
        }
    }

    public class ConditionList : List<Condition>
    {
    }

    public enum Operation
    {
        Equal,
        NotEqual,
        Contains,
        NotContains,
        GreaterThan,
        LessThan,
        RegEx
    }
}
