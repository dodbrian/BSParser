using BSParser.Parser;
using System;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace BSForms
{
    public partial class RulesForm : DockContent
    {
        RuleList _rules;

        public RulesForm()
        {
            InitializeComponent();

            _rules = new RuleList();
            ruleListBindingSource.DataSource = _rules;
        }

        private void RulesForm_Load(object sender, EventArgs e)
        {
            menuStrip1.Visible = false;
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new RuleConditionsForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                var rule = new Rule
                {
                    ValueIfTrue = form.CategoryTextBox.Text,
                    Conditions = form.Conditions,
                    Name = form.NameTextBox.Text
                };
                _rules.Add(rule);
            }

            ruleListBindingSource.ResetBindings(false);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_rules == null || !_rules.Any()) return;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            var parser = new Parser { Rules = _rules };
            parser.SaveRules(saveFileDialog1.FileName);
        }

        public void LoadFile(String fileName)
        {
            var parser = new Parser();
            parser.LoadRules(fileName);
            _rules = parser.Rules;
            ruleListBindingSource.DataSource = _rules;
            ruleListBindingSource.ResetBindings(false);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1) return;
            if (_rules == null || !_rules.Any()) return;

            var rule = (Rule)ruleListBindingSource.Current;

            var form = new RuleConditionsForm
            {
                Conditions = rule.Conditions,
                CategoryTextBox = { Text = rule.ValueIfTrue },
                NameTextBox = { Text = rule.Name }
            };

            if (form.ShowDialog() != DialogResult.OK) return;

            rule.Name = form.NameTextBox.Text;
            rule.ValueIfTrue = form.CategoryTextBox.Text;
            ruleListBindingSource.ResetBindings(false);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_rules == null || !_rules.Any()) return;

            var rule = (Rule)ruleListBindingSource.Current;
            var list = new ConditionList();

            list.AddRange(rule.Conditions.Select(cond => new Condition(cond.Field, cond.Operation, cond.TestValue)));

            var form = new RuleConditionsForm
            {
                Conditions = list,
                CategoryTextBox = { Text = rule.ValueIfTrue },
                NameTextBox = { Text = rule.Name }
            };

            if (form.ShowDialog() != DialogResult.OK) return;

            var newRule = new Rule
            {
                ValueIfTrue = form.CategoryTextBox.Text,
                Conditions = form.Conditions,
                Name = form.NameTextBox.Text
            };

            _rules.Add(newRule);
            ruleListBindingSource.ResetBindings(false);
        }
    }
}
