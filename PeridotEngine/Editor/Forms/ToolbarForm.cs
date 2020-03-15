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

        public event EventHandler BtnShowCollidersCheckedChanged
        {
            add => btnShowColliders.CheckedChanged += value;
            remove => btnShowColliders.CheckedChanged -= value;
        }

        public ToolbarForm()
        {
            InitializeComponent();
        }
    }
}
