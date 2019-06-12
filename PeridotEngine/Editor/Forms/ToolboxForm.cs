#nullable enable

using PeridotEngine.Resources;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PeridotEngine.Editor.Forms
{
    public partial class ToolboxForm : Form
    {
        public ToolboxForm()
        {
            InitializeComponent();
        }

        public void PopulateSolidsFromTextureDirectory(string directory)
        {
            ImageList il = new ImageList();

            lvSolids.LargeImageList = il;

            foreach (string filePath in Directory.GetFiles(directory, "*.ptex"))
            {
                System.Diagnostics.Debug.WriteLine(filePath);
                XElement xEle = XElement.Load(filePath);

                TextureData tex = TextureManager.LoadTexture(xEle.Element("ImagePath").Value);

                MemoryStream ms = new MemoryStream();
                tex.Texture.SaveAsPng(ms, tex.Texture.Width, tex.Texture.Height);

                il.Images.Add(tex.Name, Image.FromStream(ms));

                ListViewItem lvItem = new ListViewItem()
                {
                    Text = tex.Name,
                    Tag = tex,
                    ImageKey = tex.Name
                };

                lvSolids.Items.Add(lvItem);
            }
        }

        /// <summary>
        /// Returns the selected texture or null if none is selected.
        /// </summary>
        public TextureData? SelectedTexture
        {
            get
            {
                if (lvSolids.SelectedItems.Count > 0)
                {
                    return (TextureData)lvSolids.SelectedItems[0].Tag;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
