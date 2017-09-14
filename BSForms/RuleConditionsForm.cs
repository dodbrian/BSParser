using System;
using System.Windows.Forms;
using BSParser.Data;
using BSParser.Parser;

namespace BSForms
{
    public partial class RuleConditionsForm : Form
    {
        public ConditionList Conditions { get; set; }

        struct FieldsListboxValue
        {
            public StatementField FieldName { get; set; }
            public string FieldDescription { get; set; }

            public FieldsListboxValue(StatementField fieldName, String fieldDescription) : this()
            {
                FieldName = fieldName;
                FieldDescription = fieldDescription;
            }
        }

        struct OperationListboxValue
        {
            public string OperationDescription { get; set; }
            public Operation OperationName { get; set; }

            public OperationListboxValue(String operationDescription, Operation operationName) : this()
            {
                OperationDescription = operationDescription;
                OperationName = operationName;
            }
        }

        public TextBox CategoryTextBox { get; set; }
        public TextBox NameTextBox { get; set; }

        public RuleConditionsForm()
        {
            InitializeComponent();
        }

        private void RuleConditionsForm_Load(object sender, EventArgs e)
        {
            FieldsListboxValue[] fieldsList = 
            {
                new FieldsListboxValue(StatementField.Amount, "Сумма"),
                new FieldsListboxValue(StatementField.CreatedOn, "Дата создания"),
                new FieldsListboxValue(StatementField.Description, "Описание"),
                new FieldsListboxValue(StatementField.Direction, "Д/К"),
                new FieldsListboxValue(StatementField.DocNum, "Номер документа"),
                new FieldsListboxValue(StatementField.PayerBIK, "БИК плательщика"),
                new FieldsListboxValue(StatementField.PayerINN, "ИНН Плательщика"),
                new FieldsListboxValue(StatementField.PayerName, "Плательщик"),
                new FieldsListboxValue(StatementField.ReceiverINN, "ИНН Получателя"),
                new FieldsListboxValue(StatementField.ReceiverName, "Получатель"),
                new FieldsListboxValue(StatementField.RefNum, "Референс"),
                new FieldsListboxValue(StatementField.RegisteredOn, "Дата регистрации")
            };

            OperationListboxValue[] operationsList =
            {
                new OperationListboxValue("Содержит", Operation.Contains),
                new OperationListboxValue("Не содержит", Operation.NotContains),
                new OperationListboxValue("=", Operation.Equal),
                new OperationListboxValue("<>", Operation.NotEqual),
                new OperationListboxValue(">", Operation.GreaterThan),
                new OperationListboxValue("<", Operation.LessThan),
                new OperationListboxValue("Регулярное выражение", Operation.RegEx)
            };

            if (Conditions == null)
            {
                Conditions = new ConditionList();
                /*
                Condition condition = new Condition();
                condition.Field = StatementField.Description;
                condition.Operation = Operation.RegEx;
                conditions.Add(condition);
                */
            }

            fieldDataGridViewTextBoxColumn.DataSource = fieldsList;
            fieldDataGridViewTextBoxColumn.DisplayMember = "FieldDescription";
            fieldDataGridViewTextBoxColumn.ValueMember = "FieldName";

            operationDataGridViewTextBoxColumn.DataSource = operationsList;
            operationDataGridViewTextBoxColumn.DisplayMember = "OperationDescription";
            operationDataGridViewTextBoxColumn.ValueMember = "OperationName";

            conditionBindingSource.DataSource = Conditions;
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            var condition = new Condition
                {
                    Field = StatementField.Direction,
                    Operation = Operation.Equal,
                    TestValue = ""
                };

            Conditions.Add(condition);

            conditionBindingSource.ResetBindings(false);
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
