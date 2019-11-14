#nullable enable

using System;
using System.Windows.Forms;

namespace PeridotEngine.Editor.Forms
{
    public partial class ToolbarForm : Form
    {
        public event EventHandler MiSaveClick
        {
            add => miSave.Click += value;
            remove => miSave.Click -= value;
        }

        public ToolbarForm()
        {
            InitializeComponent();
        }

        private void ToolbarForm_Load(object sender, EventArgs e)
        {
            cmbView.SelectedIndex = 0; // set default value
        }
    }
}
