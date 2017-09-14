using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BSParser.Adapters;
using BSParser.Readers;
using WeifenLuo.WinFormsUI.Docking;
using BSParser;

namespace BSForms
{
    public partial class ProjectForm : DockContent
    {
        ProjectItemList _items;

        public class ProjectItem
        {
            public string FileName { get; set; }
            public string Path { get; set; }
            public ReaderType Type { get; set; }
        }

        public class ProjectItemList : List<ProjectItem>
        {
        }

        public ProjectForm()
        {
            InitializeComponent();
        }

        private void ProjectForm_Load(object sender, EventArgs e)
        {
            _items = new ProjectItemList();
            projectItemListBindingSource.DataSource = _items;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            var checker = new SourceChecker();

            foreach (var fileName in openFileDialog1.FileNames)
            {
                var item = new ProjectItem
                    {
                        FileName = Path.GetFileName(fileName),
                        Path = Path.GetDirectoryName(fileName)
                    };

                using (var reader = checker.Detect(fileName))
                {
                    item.Type = reader == null ? ReaderType.Unknown : reader.Type;
                }

                _items.Add(item);
            }

            projectItemListBindingSource.ResetBindings(false);
        }

        private Adapter Import()
        {
            if (_items.Any())
            {
                var adapter = new Adapter();
                var checker = new SourceChecker();

                foreach (var item in _items)
                {
                    using (var reader = checker.Detect(item.Path + '\\' + item.FileName))
                    {
                        if (reader != null)
                        {
                            adapter.Load(reader);
                        }
                    }
                }

                return adapter;
            }

            return null;
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_items.Any()) return;

            var adapter = Import();

            if (adapter == null) return;

            var form = new StatementForm { Adapter = adapter, TabText = "New Statement" };

            form.Show(DockPanel);
        }

        private void importParseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_items.Any()) return;

            var adapter = Import();

            if (adapter == null) return;

            var form = new StatementForm {Adapter = adapter, TabText = "New Statement"};

            form.Parse();
            form.Show(DockPanel);
        }
    }
}
