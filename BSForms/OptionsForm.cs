using System;
using System.Windows.Forms;

namespace BSForms
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileNameTextBox.Text = openFileDialog1.FileName;
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            fileNameTextBox.Text = Config.GetRulesFileName();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Config.SetRulesFileName(fileNameTextBox.Text);
        }
    }
}
