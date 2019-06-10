#nullable enable

using System.Windows.Forms;

namespace PeridotEngine.Editor.Forms
{
    public partial class ToolbarForm : Form
    {
        public ToolbarForm()
        {
            InitializeComponent();
        }

        private void ToolbarForm_Load(object sender, System.EventArgs e)
        {
            cmbView.SelectedIndex = 0; // set default value
        }
    }
}
