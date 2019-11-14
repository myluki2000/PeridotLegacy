#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Editor.Forms;
using PeridotEngine.Resources;
using PeridotEngine.World;
using PeridotEngine.World.WorldObjects.Solids;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using PeridotEngine.Graphics;
using PeridotEngine.World.WorldObjects;
using PeridotEngine.World.WorldObjects.Entities;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace PeridotEngine.UI
{
    class EditorScreen : Screen
    {
        private readonly ToolbarForm toolbarForm = new ToolbarForm();
        private readonly ToolboxForm toolboxForm = new ToolboxForm();

        private string levelPath;

        private IWorldObject? selectedObject;

        private Level _level;
        /// <summary>
        /// The level which is being edited.
        /// </summary>
        public Level Level
        {
            get => _level;

            set
            {
                _level = value;
                _level.Initialize();
            }
        }

        public EditorScreen(string lvlPath)
        {
            this.levelPath = lvlPath;


            // load level if it exists or create it if it doesn't
            if (File.Exists(lvlPath))
            {
                // load level
                _level = Level.FromFile(lvlPath);
                _level.Initialize();
            }
            else
            {
                // create new level
                _level = new Level();
                _level.Initialize();
            }
        }

        public override void Initialize()
        {
            toolbarForm.Show();

            toolbarForm.MiSaveClick += ToolbarForm_MiSave_Click;


            toolboxForm.Show();

            toolboxForm.ObjectWidthChanged += SelectedObjectWidthChanged;
            toolboxForm.ObjectHeightChanged += SelectedObjectHeightChanged;
            toolboxForm.ObjectZIndexChanged += SelectedObjectZIndexChanged;
            toolboxForm.PopulateSolidsFromTextureDirectory(Level.TextureDirectory);
        }

        public override void Draw(SpriteBatch sb)
        {
            Level.Draw(sb);

            DrawPreview(sb);
            DrawSelectionBox(sb);

            base.DrawUI(sb);
        }

        private KeyboardState lastKeyboardState;
        private MouseState lastMouseState;
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            Level.Update(gameTime);

            HandleCameraDrag(lastMouseState, mouseState);
            HandleCameraZoom(lastMouseState, mouseState);
            HandleObjectPlacement(lastMouseState, mouseState);
            HandleObjectSelection(lastMouseState, mouseState);
            HandleObjectDrag(lastMouseState, mouseState);

            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
        }

        private void DrawPreview(SpriteBatch sb)
        {
            if (toolboxForm.SelectedObject == null) return;

            IWorldObject obj = toolboxForm.SelectedObject;

            sb.Begin();
            // TODO: Implement preview image when placing objects
            sb.End();
        }

        private void DrawSelectionBox(SpriteBatch sb)
        {
            if (selectedObject == null) return;

            sb.Begin(transformMatrix: Level.Camera.GetMatrix());
            Utility.DrawOutline(sb, new Rectangle(selectedObject.Position.ToPoint(), selectedObject.Size.ToPoint()), Color.Red, 2);
            sb.End();
        }

        private bool dragging = false;
        private Vector2 cursorDelta;
        private void HandleObjectDrag(MouseState lastMouseState, MouseState mouseState)
        {
            if (selectedObject == null) return;

            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                // check if mouse pos is in selected object
                Vector2 mouseWorldCoords = mouseState.Position.ToVector2().Transform(Level.Camera.GetMatrix().Invert());
                if (new Rectangle(selectedObject.Position.ToPoint(), selectedObject.Size.ToPoint()).Contains(mouseWorldCoords))
                {
                    // dragging starts
                    dragging = true;
                    // calculate difference between cursor and object position
                    cursorDelta = mouseWorldCoords - selectedObject.Position;
                }
            }
            else if (lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed && dragging)
            {
                // dragging continues; update selected object position to mouse position
                selectedObject.Position = mouseState.Position.ToVector2().Transform(Level.Camera.GetMatrix().Invert()) - cursorDelta;
            }
            else
            {
                dragging = false;
            }
        }

        private void HandleObjectSelection(MouseState lastMouseState, MouseState mouseState)
        {
            // left mouse button was clicked
            if (lastMouseState.LeftButton != ButtonState.Pressed ||
                mouseState.LeftButton != ButtonState.Released) return;

            // cursor is selected
            if (toolboxForm.SelectedObject != null) return;

            Vector2 mousePosWorldSpace = Mouse.GetState().Position.ToVector2().Transform(Matrix.Invert(Level.Camera.GetMatrix()));

            IEnumerable<IEntity> entities = Level.Entities.Where(x => new Rectangle(x.Position.ToPoint(), x.Size.ToPoint()).Contains(mousePosWorldSpace));

            if (entities.Any())
            {
                selectedObject = entities.First();
                PopulateValuesFromSelectedObject();
                return;
            }

            IEnumerable<ISolid> solids = Level.Solids.Where(x => new Rectangle(x.Position.ToPoint(), x.Size.ToPoint()).Contains(mousePosWorldSpace));

            if (solids.Any())
            {
                selectedObject = solids.First();
                PopulateValuesFromSelectedObject();
                return;
            }

            selectedObject = null;
        }

        private void PopulateValuesFromSelectedObject()
        {
            if (selectedObject == null) return;

            toolboxForm.ObjectWidth = (int)selectedObject.Size.X;
            toolboxForm.ObjectHeight = (int)selectedObject.Size.Y;
            toolboxForm.ObjectZIndex = selectedObject.ZIndex;
        }

        private void HandleCameraDrag(MouseState lastMouseState, MouseState mouseState)
        {
            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                Level.Camera.Translation = new Vector3(
                    Level.Camera.Translation.X + mouseState.Position.X - lastMouseState.Position.X,
                    Level.Camera.Translation.Y + mouseState.Position.Y - lastMouseState.Position.Y,
                    0
                );
            }
        }

        private void HandleCameraZoom(MouseState lastMouseState, MouseState mouseState)
        {
            int scrollDelta = mouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue;
            if (scrollDelta > 0)
            {
                Level.Camera.Translation -= new Vector3(1920 / 2, 1080 / 2, 0) / Level.Camera.Scale;
                Level.Camera.Scale *= new Vector3(1.25f, 1.25f, 1);
                Level.Camera.Translation += new Vector3(1920 / 2, 1080 / 2, 0) / Level.Camera.Scale;
            }
            else if (scrollDelta < 0)
            {
                Level.Camera.Translation -= new Vector3(1920 / 2, 1080 / 2, 0) / Level.Camera.Scale;
                Level.Camera.Scale /= new Vector3(1.25f, 1.25f, 1);
                Level.Camera.Translation += new Vector3(1920 / 2, 1080 / 2, 0) / Level.Camera.Scale;
            }
        }

        private void HandleObjectPlacement(MouseState lastMouseState, MouseState mouseState)
        {
            // place if left mouse button was pressed
            if (lastMouseState.LeftButton != ButtonState.Pressed || mouseState.LeftButton != ButtonState.Released) return;

            // check if any object is selected
            if (toolboxForm.SelectedObject == null) return;

            if (toolboxForm.SelectedObject is ISolid solid)
            {
                solid.Initialize(Level);
                solid.Position = Level.Camera.ScreenPosToWorldPos(mouseState.Position.ToVector2());
                Level.Solids.Add(solid);
            }
            else if (toolboxForm.SelectedObject is IEntity entity)
            {
                entity.Initialize(Level);
                entity.Position = Level.Camera.ScreenPosToWorldPos(mouseState.Position.ToVector2());
                Level.Entities.Add(entity);
            }
        }

        private void ToolbarForm_MiSave_Click(object sender, EventArgs e)
        {
            Level.ToFile(levelPath);
        }

        private void SelectedObjectWidthChanged(object sender, int value)
        {
            if (selectedObject != null) selectedObject.Size = new Vector2(value, selectedObject.Size.Y);
        }

        private void SelectedObjectHeightChanged(object sender, int value)
        {
            if (selectedObject != null) selectedObject.Size = new Vector2(selectedObject.Size.X, value);
        }

        private void SelectedObjectZIndexChanged(object sender, sbyte value)
        {
            if (selectedObject != null) selectedObject.ZIndex = value;
        }
    }
}
