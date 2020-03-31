using System.Windows.Forms;

namespace PeridotEngine.Engine.Editor.Forms
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
