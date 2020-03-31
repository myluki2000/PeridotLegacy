#nullable enable

using System;
using System.Windows.Forms;

namespace PeridotEngine.Engine.Editor.Forms
{
    public partial class ToolbarForm : Form
    {
        public event EventHandler MiSaveClick
        {
            add => miSave.Click += value;
            remove => miSave.Click -= value;
        }

        public event EventHandler BtnEditCollidersCheckedChanged
        {
            add => btnEditColliders.CheckedChanged += value;
            remove => btnEditColliders.CheckedChanged -= value;
        }

        public bool BtnEditCollidersChecked
        {
            get => btnEditColliders.Checked;
            set => btnEditColliders.Checked = value;
        }

        public ToolbarForm()
        {
            InitializeComponent();
        }
    }
}
