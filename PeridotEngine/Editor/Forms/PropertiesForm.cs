using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeridotEngine.Editor.Forms
{
    public partial class PropertiesForm : Form
    {
        public PropertiesForm()
        {
            InitializeComponent();
        }

        public object SelectedObject
        {
            get => pgProperties.SelectedObject;
            set => pgProperties.SelectedObject = value;
        }
    }
}
