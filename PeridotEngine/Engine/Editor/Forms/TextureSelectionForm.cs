#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.Editor.Forms
{
    public partial class TextureSelectionForm : Form
    {

        public TextureDataBase? SelectedTexture { get; private set; }

        public TextureSelectionForm(string directory)
        {
            InitializeComponent();

            PopulateTexturesFromTextureDirectory(directory);
        }

        /// <summary>
        /// Populates the solids list with textures from a directory. 
        /// </summary>
        /// <param name="directory">The directory to populate from</param>
        private void PopulateTexturesFromTextureDirectory(string directory)
        {
            // cancel loading if there is no texture directory to load
            if (directory == null || !Directory.Exists(directory))
            {
                return;
            }
            
            ImageList il = new ImageList();

            lvTextures.LargeImageList = il;

            foreach (string filePath in Directory.GetFiles(directory, "*.ptex"))
            {
                XElement xEle = XElement.Load(filePath);

                TextureDataBase tex = TextureManager.LoadTexture(xEle.Element("ImagePath").Value);

                il.Images.Add(tex.Name, tex.Texture.ToImage(150, 150));

                ListViewItem lvItem = new ListViewItem()
                {
                    Text = tex.Name,
                    Tag = tex,
                    ImageKey = tex.Name
                };

                lvTextures.Items.Add(lvItem);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void lvTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvTextures.SelectedItems.Count > 0)
            {
                SelectedTexture = (TextureDataBase)lvTextures.SelectedItems[0].Tag;
            }
            else
            {
                SelectedTexture = null;
            }
        }

        private void lvTextures_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvTextures.SelectedItems.Count > 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
