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

        public Material? SelectedMaterial { get; private set; }

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

            foreach (string filePath in Directory.GetFiles(directory, "*.pmat"))
            {
                Material mat = TextureManager.LoadMaterial(filePath);

                il.Images.Add(mat.Name, mat.Textures[(int)Material.TextureType.Diffuse].Texture.ToImage(150, 150));

                ListViewItem lvItem = new ListViewItem()
                {
                    Text = mat.Name,
                    Tag = mat,
                    ImageKey = mat.Name
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
                SelectedMaterial = (Material)lvTextures.SelectedItems[0].Tag;
            }
            else
            {
                SelectedMaterial = null;
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
