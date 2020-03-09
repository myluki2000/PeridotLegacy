﻿#nullable enable

using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using PeridotEngine.Resources;
using PeridotEngine.World.WorldObjects;
using PeridotEngine.World.WorldObjects.Solids;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using PeridotEngine.World.WorldObjects.Entities;

namespace PeridotEngine.Editor.Forms
{
    public partial class ToolboxForm : Form
    {
        /// <summary>
        /// Gets or sets the z-index of the z-index control in the form.
        /// </summary>
        public sbyte ObjectZIndex { get => (sbyte)nudZIndex.Value; set => nudZIndex.Value = value; }
        /// <summary>
        /// Gets or sets the value set for object width in the toolbox.
        /// </summary>
        public int ObjectWidth { get => (int) nudWidth.Value; set => nudWidth.Value = value; }
        /// <summary>
        /// Gets or sets the value set for object height in the toolbox.
        /// </summary>
        public int ObjectHeight { get => (int) nudHeight.Value; set => nudHeight.Value = value; }
        /// <summary>
        /// Returns the selected texture or null if none is selected.
        /// </summary>
        public IWorldObject? SelectedObject
        {
            get
            {
                Vector2 size = new Vector2((int) nudWidth.Value, (int) nudHeight.Value);
                if (lvSolids.SelectedItems.Count > 0)
                {

                    if ((string)lvSolids.SelectedItems[0].Text == "Dynamic Water")
                    {
                        DynamicWater obj = new DynamicWater()
                        {
                            Size = size
                        };

                        return obj;
                    }
                    else
                    {
                        return new TexturedSolid()
                        {
                            Texture = (TextureData)lvSolids.SelectedItems[0].Tag,
                            Size = size
                        };
                    }

                }
                else if (lvEntities.SelectedItems.Count > 0)
                {
                    String s = lvEntities.SelectedItems[0].Text;
                    switch (s)
                    {
                        case "Player":
                            return new Player() {Size = size};

                        default:
                            return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public event EventHandler<sbyte>? ObjectZIndexChanged;
        public event EventHandler<int>? ObjectWidthChanged;
        public event EventHandler<int>? ObjectHeightChanged;

        public ToolboxForm()
        {
            InitializeComponent();
            PopulateDefaultSolids();
            PopulateDefaultEntities();
        }

        /// <summary>
        /// Populates the solids list with textures from a directory. 
        /// </summary>
        /// <param name="directory">The directory to populate from</param>
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

        private void PopulateDefaultSolids()
        {
            lvSolids.Items.Add("Dynamic Water");
        }

        private void PopulateDefaultEntities()
        {
            lvEntities.Items.Add("Player");
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

        private void BtnCursor_Click(object sender, System.EventArgs e)
        {
            lvSolids.SelectedIndices.Clear();
            lvEntities.SelectedIndices.Clear();
        }

        private void NudZIndex_ValueChanged(object sender, EventArgs e)
        {
            ObjectZIndexChanged?.Invoke(this, (sbyte)nudZIndex.Value);
        }

        private void NudWidth_ValueChanged(object sender, EventArgs e)
        {
            ObjectWidthChanged?.Invoke(this, (int)nudWidth.Value);
        }

        private void NudHeight_ValueChanged(object sender, EventArgs e)
        {
            ObjectHeightChanged?.Invoke(this, (int) nudHeight.Value);
        }
    }
}
