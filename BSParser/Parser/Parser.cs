using System;
using System.Collections.Generic;
using System.Linq;
using BSParser.Data;
using System.Xml.Linq;

namespace BSParser.Parser
{
    public class Parser
    {
        public RuleList Rules { get; set; }

        public void Parse(StatementTable data)
        {
            foreach (var row in data)
            {
                foreach (var rule in Rules.Where(rule => rule.Check(row)))
                {
                    row.Category = rule.ValueIfTrue;
                    break;
                }
            }
        }

        public void SaveRules(String fileName)
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("BSParser Rules"),
                new XComment("Version 1.0"),
                new XComment("Created: " + DateTime.Now),
                GetRules()
                );

            doc.Save(fileName);
        }

        public void LoadRules(String fileName)
        {
            var tmpRules = new RuleList();
            var doc = XDocument.Load(fileName);

            var rulesList = from list in doc.Elements("Rules").Elements()
                            where list.Name == "Rule"
                            select list;

            tmpRules.AddRange(rulesList.Select(rule =>
                {
                    var nameAttribute = rule.Attribute("Name");
                    var trueValAttribute = rule.Attribute("ValueIfTrue");

                    if (trueValAttribute != null && nameAttribute != null)
                        return new Rule(nameAttribute.Value, trueValAttribute.Value)
                            {
                                Conditions = SetConditions(rule)
                            };

                    return null;
                }));

            Rules = tmpRules;
        }

        static ConditionList SetConditions(XContainer rule)
        {
            var conditions = new ConditionList();

            var conditionList = from list in rule.Elements("Conditions").Elements()
                                where list.Name == "Condition"
                                select list;

            conditions.AddRange(from condition in conditionList
                                let fieldAttribute = condition.Attribute("Field")
                                let operationAttribute = condition.Attribute("Operation")
                                let testValAttribute = condition.Attribute("TestValue")
                                where testValAttribute != null && operationAttribute != null && fieldAttribute != null
                                select
                                    new Condition(fieldAttribute.Value, operationAttribute.Value, testValAttribute.Value));
            return conditions;
        }

        XElement GetRules()
        {
            var rulesList = new XElement("Rules");

            foreach (var rule in Rules)
            {
                rulesList.Add(new XElement("Rule",
                    new XAttribute("Name", rule.Name),
                    new XAttribute("ValueIfTrue", rule.ValueIfTrue),
                    GetConditions(rule.Conditions))
                    );
            }

            return rulesList;
        }

        static XElement GetConditions(IEnumerable<Condition> conditions)
        {
            var conditionsList = new XElement("Conditions");

            foreach (var condition in conditions)
            {
                conditionsList.Add(new XElement("Condition",
                    new XAttribute("Field", condition.Field),
                    new XAttribute("Operation", condition.Operation),
                    new XAttribute("TestValue", condition.TestValue)
                    ));
            }

            return conditionsList;
        }
    }
}
