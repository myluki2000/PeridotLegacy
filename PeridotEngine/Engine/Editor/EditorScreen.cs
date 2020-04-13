#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Engine.Editor.Forms;
using PeridotEngine.Engine.Editor.Forms.PropertiesForm;
using PeridotEngine.Engine.UI;
using PeridotEngine.Engine.Utility;
using PeridotEngine.Engine.World;
using PeridotEngine.Engine.World.Physics.Colliders;
using PeridotEngine.Engine.World.WorldObjects;
using PeridotEngine.Engine.World.WorldObjects.Entities;
using PeridotEngine.Engine.World.WorldObjects.Solids;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace PeridotEngine.Engine.Editor
{
    class EditorScreen : Screen
    {
        private readonly ToolbarForm toolbarForm = new ToolbarForm();
        private readonly ToolboxForm toolboxForm;
        private readonly PropertiesForm propertiesForm;

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

            toolboxForm = new ToolboxForm(level.TextureDirectory);
            propertiesForm = new PropertiesForm(level.TextureDirectory);
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

            propertiesForm.Show();
        }

        public override void Draw(SpriteBatch sb)
        {
            Level.Draw(sb);

            sb.Begin();

            DrawPreview(sb);
            DrawSelectionBox(sb);

            sb.End();

            sb.Begin(transformMatrix: Level.Camera.GetMatrix());

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
            selectedObject?.DrawOutline(sb, Color.Red, Level.Camera);
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

            IWorldObject obj = toolboxForm.SelectedObject;
            
            obj.Initialize(Level);
            obj.Position = Level.Camera.ScreenPosToWorldPos(mouseState.Position.ToVector2());
            Level.WorldObjects.Add(obj);
            
           
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
                if (selectedObject.ContainsPointOnScreen(mouseState.Position, Level.Camera))
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

            IWorldObject obj = Level.WorldObjects.FirstOrDefault(x => x.ContainsPointOnScreen(mouseState.Position, Level.Camera));

            if (obj != null)
            {
                selectedObject = obj;
                propertiesForm.SelectedObject = selectedObject;
                PopulateValuesFromSelectedObject();
                return;
            }

            selectedObject = null;
            propertiesForm.SelectedObject = null;
        }

        private void HandleObjectDeletion(KeyboardState lastKeyboardState, KeyboardState keyboardState)
        {
            if (lastKeyboardState.IsKeyDown(Keys.Delete) && keyboardState.IsKeyUp(Keys.Delete))
            {
                if (selectedObject != null)
                {
                    Level.WorldObjects.Remove(selectedObject);
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
                        quadCollider.Quad.Point2 = mousePosInLevel;
                        quadCollider.Quad.Point4 = ((Point)colliderStart).ToVector2();

                        quadCollider.Quad.Point1 = new Vector2(quadCollider.Quad.Point4.X, quadCollider.Quad.Point2.Y);
                        quadCollider.Quad.Point3 = new Vector2(quadCollider.Quad.Point2.X, quadCollider.Quad.Point4.Y);

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
                Utility.Utility.DrawOutline(
                    sb,
                    new Rectangle((Point)colliderStart, Level.Camera.ScreenPosToWorldPos(Mouse.GetState().Position.ToVector2()).ToPoint() - (Point)colliderStart).Transform(Level.Camera.GetMatrix()),
                    Color.Red
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
