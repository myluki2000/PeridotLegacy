#nullable enable

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Editor.Forms;
using PeridotEngine.World;
using PeridotEngine.World.WorldObjects.Solids;
using System.IO;
using System.Linq;
using PeridotEngine.Graphics;
using PeridotEngine.Misc;
using PeridotEngine.World.Physics.Colliders;
using PeridotEngine.World.WorldObjects;
using PeridotEngine.World.WorldObjects.Entities;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace PeridotEngine.UI
{
    class EditorScreen : Screen
    {
        private readonly ToolbarForm toolbarForm = new ToolbarForm();
        private readonly ToolboxForm toolboxForm = new ToolboxForm();
        private readonly PropertiesForm propertiesForm = new PropertiesForm();

        private readonly string levelPath;

        private IWorldObject? selectedObject;
        private ICollider? selectedCollider;

        private Level level;
        /// <summary>
        /// The level which is being edited.
        /// </summary>
        public Level Level
        {
            get => level;

            set
            {
                level = value;

                // disable physics updating for this level
                level.IsPhysicsEnabled = false;

                level.Initialize();
            }
        }

        public EditorScreen(string lvlPath)
        {
            this.levelPath = lvlPath;

            Level tmpLevel;
            // load level if it exists or create it if it doesn't
            if (File.Exists(lvlPath))
            {
                // load level
                tmpLevel = Level.FromFile(lvlPath);
            }
            else
            {
                // create new level
                tmpLevel = new Level();
            }

            level = tmpLevel;

            // disable physics updating for this level
            level.IsPhysicsEnabled = false;
            // disable camera following player
            level.CameraShouldFollowPlayer = false;

            level.Initialize();
        }

        public override void Initialize()
        {
            toolbarForm.Show();

            toolbarForm.MiSaveClick += ToolbarForm_MiSave_Click;
            toolbarForm.BtnEditCollidersCheckedChanged += ToolbarForm_BtnEditColliders_CheckedChanged;

            toolboxForm.Show();

            toolboxForm.ObjectWidthChanged += SelectedObjectWidthChanged;
            toolboxForm.ObjectHeightChanged += SelectedObjectHeightChanged;
            toolboxForm.ObjectZIndexChanged += SelectedObjectZIndexChanged;
            toolboxForm.PopulateSolidsFromTextureDirectory(Level.TextureDirectory);

            propertiesForm.Show();
        }

        public override void Draw(SpriteBatch sb)
        {
            Level.Draw(sb);

            sb.Begin(transformMatrix: Level.Camera.GetMatrix());

            DrawPreview(sb);
            DrawSelectionBox(sb);

            if (toolbarForm.BtnEditCollidersChecked)
            {
                DrawColliders(sb);
                DrawColliderPreview(sb);
            }

            sb.End();

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

            // handle collider specific edit moves when in collider edit mode, handle default edit moves otherwise
            if (toolbarForm.BtnEditCollidersChecked)
            {
                if (mouseState.IsInWindow())
                {
                    HandleColliderPlacement(lastMouseState, mouseState);
                    HandleColliderSelection(lastMouseState, mouseState);
                    
                    selectedCollider?.HandleDraggingAndResizing(Level, lastMouseState, mouseState);
                }
            }
            else
            {
                if (mouseState.IsInWindow())
                {
                    HandleObjectPlacement(lastMouseState, mouseState);
                    HandleObjectSelection(lastMouseState, mouseState);
                    HandleObjectDrag(lastMouseState, mouseState);
                }
                HandleObjectDeletion(lastKeyboardState, keyboardState);
            }


            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
        }

        private void DrawColliders(SpriteBatch sb)
        {
            // draw the colliders
            foreach (ICollider collider in Level.Colliders)
            {
                // draw the collider with a red color if it is selected and green otherwise
                collider.Draw(sb, Level.Camera, collider == selectedCollider ? Color.Red : Color.Green, collider == selectedCollider);
            }
        }


        #region Object Editing Handling

        private void DrawSelectionBox(SpriteBatch sb)
        {
            if (selectedObject == null) return;

            Utility.DrawOutline(sb, new Rectangle(selectedObject.Position.ToPoint(), selectedObject.Size.ToPoint()), Color.Red, 2);
        }

        private void DrawPreview(SpriteBatch sb)
        {
            if (toolboxForm.SelectedObject == null) return;

            IWorldObject obj = toolboxForm.SelectedObject;

            // TODO: Implement preview image when placing objects
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

            Vector2 mousePosWorldSpace = Level.Camera.ScreenPosToWorldPos(Mouse.GetState().Position.ToVector2());
            Console.WriteLine(mousePosWorldSpace);
            IEnumerable<IEntity> entities = Level.Entities.Where(x => new Rectangle(x.Position.ToPoint(), x.Size.ToPoint()).Contains(mousePosWorldSpace));

            if (entities.Any())
            {
                selectedObject = entities.First();
                propertiesForm.SelectedObject = selectedObject;
                PopulateValuesFromSelectedObject();
                return;
            }

            IEnumerable<ISolid> solids = Level.Solids.Where(x => new Rectangle(x.Position.ToPoint(), x.Size.ToPoint()).Contains(mousePosWorldSpace));

            if (solids.Any())
            {
                selectedObject = solids.First();
                propertiesForm.SelectedObject = selectedObject;
                PopulateValuesFromSelectedObject();
                return;
            }

            selectedObject = null;
        }

        private void HandleObjectDeletion(KeyboardState lastKeyboardState, KeyboardState keyboardState)
        {
            if (lastKeyboardState.IsKeyDown(Keys.Delete) && keyboardState.IsKeyUp(Keys.Delete))
            {
                if (selectedObject != null)
                {
                    if (selectedObject is IEntity) Level.Entities.Remove((IEntity)selectedObject);
                    if (selectedObject is ISolid) Level.Solids.Remove((ISolid)selectedObject);
                    selectedObject = null;
                }
            }
        }

        #endregion


        #region Collider Editing Handling

        private Point? colliderStart = null;
        private void HandleColliderPlacement(MouseState lastMouseState, MouseState mouseState)
        {
            if (lastMouseState.LeftButton == ButtonState.Released
                && mouseState.LeftButton == ButtonState.Pressed)
            {
                // user starts pressing mouse
                colliderStart = Level.Camera.ScreenPosToWorldPos(mouseState.Position.ToVector2()).ToPoint();
            }
            else if (lastMouseState.LeftButton == ButtonState.Pressed
                     && mouseState.LeftButton == ButtonState.Released)
            {
                if (colliderStart != null && toolboxForm.SelectedCollider != null)
                {
                    Vector2 mousePosInLevel = Level.Camera.ScreenPosToWorldPos(mouseState.Position.ToVector2());
                    // user releases mouse
                    if (toolboxForm.SelectedCollider is RectCollider rectCollider)
                    {
                        rectCollider.Rect = new Rectangle((Point)colliderStart, mousePosInLevel.ToPoint() - (Point)colliderStart);
                        Level.Colliders.Add(rectCollider);
                        colliderStart = null;
                    }
                    else if (toolboxForm.SelectedCollider is QuadCollider quadCollider)
                    {
                        quadCollider.Point2 = mousePosInLevel;
                        quadCollider.Point4 = ((Point)colliderStart).ToVector2();

                        quadCollider.Point1 = new Vector2(quadCollider.Point4.X, quadCollider.Point2.Y);
                        quadCollider.Point3 = new Vector2(quadCollider.Point2.X, quadCollider.Point4.Y);

                        Level.Colliders.Add(quadCollider);
                        colliderStart = null;
                    }
                    else
                    {
                        throw new Exception("Selected collider not supported! Is the implementation missing?");
                    }
                }
            }

        }

        private void HandleColliderSelection(MouseState lastMouseState, MouseState mouseState)
        {
            if (lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                foreach (ICollider collider in Level.Colliders)
                {
                    if (collider.Contains(Level.Camera.ScreenPosToWorldPos(mouseState.Position.ToVector2()).ToPoint()))
                    {
                        selectedCollider = collider;
                        return;
                    }
                }

                selectedCollider = null;
            }
        }

        private void DrawColliderPreview(SpriteBatch sb)
        {
            if (colliderStart != null && toolboxForm.SelectedCollider != null)
            {
                Utility.DrawOutline(
                    sb,
                    new Rectangle((Point)colliderStart, Level.Camera.ScreenPosToWorldPos(Mouse.GetState().Position.ToVector2()).ToPoint() - (Point)colliderStart),
                    Color.Red,
                    2
                );
            }
        }

        #endregion


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

        private void ToolbarForm_MiSave_Click(object sender, EventArgs e)
        {
            Level.ToFile(levelPath);
        }

        private void ToolbarForm_BtnEditColliders_CheckedChanged(object sender, EventArgs e)
        {
            toolboxForm.ColliderEditMode = toolbarForm.BtnEditCollidersChecked;
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
