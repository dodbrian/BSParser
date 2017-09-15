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

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            try
            {
                var form = FormLoader.FindForm(openFileDialog1.FileName);

                if (form != null)
                {
                    form.Show(dockPanel1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to find form:" + Environment.NewLine + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
