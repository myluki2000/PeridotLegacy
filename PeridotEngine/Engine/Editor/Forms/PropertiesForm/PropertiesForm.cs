using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using PeridotEngine.Engine.Resources;

namespace PeridotEngine.Engine.Editor.Forms.PropertiesForm
{
    public partial class PropertiesForm : Form
    {
        public PropertiesForm(string textureDirectory)
        {
            InitializeComponent();
            TextureEditor.TextureDirectory = textureDirectory;
        }

        public object SelectedObject
        {
            get => pgProperties.SelectedObject;
            set => pgProperties.SelectedObject = value;
        }
    }
}
