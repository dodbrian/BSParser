using System;
using System.Collections.Generic;
using BSParser.Data;

namespace BSParser.Parser
{
    /// <summary>
    /// Represents a parsing rule
    /// </summary>
    public class Rule
    {
        public ConditionList Conditions { get; set; }

        public string ValueIfTrue { get; set; }

        public string Name { get; set; }

        public Rule()
        { 
        }

        public Rule(String name, String valueIfTrue)
        {
            Name = name;
            ValueIfTrue = valueIfTrue;
        }

        public bool Check(StatementRow rowToCheck)
        {
            var result = true;

            foreach (Condition condition in Conditions)
            {
                switch (condition.Field)
                {
                    case StatementField.Amount:
                        result &= condition.Check(rowToCheck.Amount);
                        break;
                    case StatementField.CreatedOn:
                        result &= condition.Check(rowToCheck.CreatedOn);
                        break;
                    case StatementField.Description:
                        result &= condition.Check(rowToCheck.Description);
                        break;
                    case StatementField.Direction:
                        result &= condition.Check(rowToCheck.Direction);
                        break;
                    case StatementField.DocNum:
                        result &= condition.Check(rowToCheck.DocNum);
                        break;
                    case StatementField.PayerBIK:
                        result &= condition.Check(rowToCheck.PayerBIK);
                        break;
                    case StatementField.PayerINN:
                        result &= condition.Check(rowToCheck.PayerINN);
                        break;
                    case StatementField.PayerName:
                        result &= condition.Check(rowToCheck.PayerName);
                        break;
                    case StatementField.ReceiverINN:
                        result &= condition.Check(rowToCheck.ReceiverINN);
                        break;
                    case StatementField.ReceiverName:
                        result &= condition.Check(rowToCheck.ReceiverName);
                        break;
                    case StatementField.RefNum:
                        result &= condition.Check(rowToCheck.RefNum);
                        break;
                    case StatementField.RegisteredOn:
                        result &= condition.Check(rowToCheck.RegisteredOn);
                        break;
                }
            }

            return result;
        }

    }

    public class RuleList : List<Rule>
    {
    }
}
