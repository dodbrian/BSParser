using BSForms.Extensions;
using BSParser;
using BSParser.Adapters;
using BSParser.Writers;
using System;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace BSForms
{
    public partial class StatementForm : DockContent
    {
        private String fileName;

        private Adapter adapter;

        private void ParseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parse();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (adapter.GetTransactions().Count <= 0) return;

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            Writer writer = null;

            switch (saveFileDialog.FilterIndex)
            {
                case 1:
                    writer = new StrictCSVWriter(saveFileDialog.FileName);
                    break;
                case 2:
                    writer = new CSVWriter(saveFileDialog.FileName);
                    break;
            }

            adapter.Export(writer);
        }

        public Adapter Adapter
        {
            get { return adapter; }
            set
            {
                adapter = value;
                dataGridView1.DataSource = adapter.GetTransactions();
            }
        }

        public StatementForm()
        {
            InitializeComponent();
            dataGridView1.DoubleBuffered(true);
        }

        public void LoadFile(String fileName)
        {
            this.fileName = fileName;
            TabText = Path.GetFileName(fileName);

            var checker = new SourceChecker();

            using (var reader = checker.Detect(fileName))
            {
                if (reader == null) return;
                adapter = new Adapter();
                adapter.Load(reader);
                dataGridView1.DataSource = adapter.GetTransactions();
            }
        }

        public void Parse()
        {
            var parser = new BSParser.Parser.Parser();

            try
            {
                parser.LoadRules(Config.GetRulesFileName());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rules:" + Environment.NewLine + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            parser.Parse(adapter.GetTransactions());
            dataGridView1.Refresh();
        }
    }
}
