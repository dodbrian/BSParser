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
        String fileName;

        Adapter adapter;
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
            parser.LoadRules(Config.GetRulesFileName());

            parser.Parse(adapter.GetTransactions());
            dataGridView1.Refresh();
        }

        private void parseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parse();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (adapter.GetTransactions().Count <= 0) return;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            var writer = new CSVWriter(saveFileDialog1.FileName);
            adapter.Export(writer);
        }
    }
}
