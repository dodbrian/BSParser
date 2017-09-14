using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace BSForms
{
    public partial class MainMDIForm : Form
    {
        public MainMDIForm()
        {
            InitializeComponent();
        }

        private void MainMDIForm_Load(object sender, EventArgs e)
        {
            var form = new ProjectForm();
            form.Show(dockPanel1);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new RulesForm();
            form.Show(dockPanel1);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            var form = FormLoader.FindForm(openFileDialog1.FileName);
            if (form != null)
            {
                form.Show(dockPanel1);
            }
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ProjectForm();
            form.Show(dockPanel1);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new OptionsForm();
            form.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var box = new AboutBox();
            box.ShowDialog();
        }
    }
}
