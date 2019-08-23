#nullable enable

using Microsoft.Xna.Framework;
using PeridotEngine.Resources;
using PeridotEngine.World.WorldObjects;
using PeridotEngine.World.WorldObjects.Solids;
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
            PopulateDefaultSolids();
        }

        private void PopulateDefaultSolids()
        {
            ListViewItem lvItem = new ListViewItem()
            {
                Text = "Dynamic Water"
            };

            lvSolids.Items.Add(lvItem);
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
        public IWorldObject? SelectedObject
        {
            get
            {
                if (lvSolids.SelectedItems.Count > 0)
                {

                    if ((string)lvSolids.SelectedItems[0].Text == "Dynamic Water")
                    {
                        DynamicWater obj = new DynamicWater()
                        {
                            Size = new Vector2((int)nudWidth.Value, (int)nudHeight.Value)
                        };

                        return obj;
                    }
                    else
                    {
                        return new TexturedObject()
                        {
                            Texture = (TextureData)lvSolids.SelectedItems[0].Tag,
                            Size = new Vector2((int)nudWidth.Value, (int)nudHeight.Value)
                        };
                    }

                }
                else if(lvEntities.SelectedItems.Count > 0)
                {
                    // TODO: Implement selection of entities
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        private void LvSolids_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(lvSolids.SelectedItems.Count > 0 && lvSolids.SelectedItems[0].Tag != null)
            {
                TextureData selectedTexture = (TextureData)lvSolids.SelectedItems[0].Tag;

                nudWidth.Value = selectedTexture.Width;
                nudHeight.Value = selectedTexture.Height;
            }
        }
    }
}
