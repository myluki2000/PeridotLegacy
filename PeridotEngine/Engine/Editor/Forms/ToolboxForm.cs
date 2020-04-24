#nullable enable

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.World.Physics.Colliders;
using PeridotEngine.Engine.World.WorldObjects;
using PeridotEngine.Engine.World.WorldObjects.Entities;
using PeridotEngine.Engine.World.WorldObjects.Solids;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.Editor.Forms
{
    public partial class ToolboxForm : Form
    {
        /// <summary>
        /// Gets or sets the collider edit mode property. If true the toolbox will show tools useful for editing colliders, if it
        /// is false the toolbox will show the default tools.
        /// </summary>
        public bool ColliderEditMode
        {
            get => colliderEditMode;
            set
            {
                colliderEditMode = value;
                if (colliderEditMode)
                {
                    tabControl.Visible = false;
                    lvColliders.Visible = true;
                }
                else
                {
                    tabControl.Visible = true;
                    lvColliders.Visible = false;
                }
            }
        }

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
                IWorldObject? obj = null;
                if (lvSolids.SelectedItems.Count > 0)
                    obj = (IWorldObject) Activator.CreateInstance((Type)lvSolids.SelectedItems[0].Tag);

                if (lvEntities.SelectedItems.Count > 0)
                    obj = (IWorldObject) Activator.CreateInstance((Type) lvEntities.SelectedItems[0].Tag);


                if (obj != null)
                {
                    obj.Size = new Vector2(ObjectWidth, ObjectHeight);
                }

                if (obj is ITextured texturedObj)
                {
                    if (SelectedMaterial == null) return null;
                    texturedObj.Material = SelectedMaterial;
                }

                return obj;
            }
        }

        /// <summary>
        /// Returns the selected collider or null if none is selected.
        /// </summary>
        public ICollider? SelectedCollider
        {
            get
            {
                if (lvColliders.SelectedItems.Count > 0)
                    return (ICollider) Activator.CreateInstance((Type) lvColliders.SelectedItems[0].Tag);

                return null;
            }
        }

        /// <summary>
        /// The selected texture that will be used as the texture of newly placed (textured) objects.
        /// </summary>
        public Material? SelectedMaterial
        {
            get => selectedMaterial;
            private set
            {
                selectedMaterial = value;
                pbSelectedTexture.Image = selectedMaterial?.Textures[(int)Material.TextureType.Diffuse].Texture.ToImage(250, 250);
                lblTexturePath.Text = selectedMaterial?.Name;
            }
        }
        private Material? selectedMaterial = null;

        public event EventHandler<sbyte>? ObjectZIndexChanged;
        public event EventHandler<int>? ObjectWidthChanged;
        public event EventHandler<int>? ObjectHeightChanged;

        private bool colliderEditMode;

        private readonly string textureDirectory;

        public ToolboxForm(string textureDirectory)
        {
            InitializeComponent();

            this.textureDirectory = textureDirectory;

            PopulateSolids();
            PopulateEntities();
            PopulateColliders();
        }

        private void PopulateSolids()
        {
            lvSolids.Items.AddRange(
                Assembly.GetExecutingAssembly().GetTypes()
                    .Where(x => x.IsClass
                        && !x.IsAbstract
                        && !x.IsNestedPrivate
                        && (x.Namespace == "PeridotEngine.Engine.World.WorldObjects.Solids"
                            || x.Namespace == "PeridotEngine.Game.World.WorldObjects.Solids"))
                    .Select(x => new ListViewItem()
                    {
                        Text = x.Name,
                        Tag = x
                    })
                    .ToArray()
            );
        }

        private void PopulateEntities()
        {
            lvEntities.Items.AddRange(
                Assembly.GetExecutingAssembly().GetTypes()
                    .Where(x => x.IsClass
                        && !x.IsAbstract
                        && !x.IsNestedPrivate
                        && (x.Namespace == "PeridotEngine.Engine.World.WorldObjects.Entities"
                            || x.Namespace == "PeridotEngine.Game.World.WorldObjects.Entities"))
                    .Select(x => new ListViewItem()
                    {
                        Text = x.Name,
                        Tag = x
                    })
                    .ToArray()
            );
        }

        private void PopulateColliders()
        {
            lvColliders.Items.AddRange(
                Assembly.GetExecutingAssembly().GetTypes()
                    .Where(x => x.IsClass
                        && !x.IsAbstract
                        && !x.IsNestedPrivate
                        && (x.Namespace == "PeridotEngine.Engine.World.Physics.Colliders"
                            || x.Namespace == "PeridotEngine.Game.World.Physics.Colliders"))
                    .Select(x => new ListViewItem()
                    {
                        Text = x.Name,
                        Tag = x
                    })
                    .ToArray()
            );
        }

        private void BtnCursor_Click(object sender, EventArgs e)
        {
            lvSolids.SelectedIndices.Clear();
            lvEntities.SelectedIndices.Clear();
            lvColliders.SelectedIndices.Clear();
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

        private void BtnSelectTexture_Click(object sender, EventArgs e)
        {
            using TextureSelectionForm textureSelectionForm = new TextureSelectionForm(textureDirectory);
            DialogResult result = textureSelectionForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                SelectedMaterial = textureSelectionForm.SelectedMaterial;

                if (SelectedMaterial != null)
                {
                    ObjectWidth = SelectedMaterial.Width;
                    ObjectHeight = SelectedMaterial.Height;
                }
            }
        }
    }
}
