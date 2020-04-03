#nullable enable

using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Utility;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace PeridotEngine.Engine.World.Physics.Colliders
{
    public class RectCollider : ICollider
    {
        public Rectangle Rect
        {
            get => rect;
            set => rect = value;
        }

        private const int DRAG_POINT_SIZE = 10;

        private Corner currentlyDraggingCorner = Corner.NONE;
        private Rectangle rect;

        public bool IsColliding(Rectangle otherRect)
        {
            return Rect.Intersects(otherRect);
        }

        /// <inheritdoc />
        public bool Contains(Point point)
        {
            return Rect.Contains(point);
        }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera, Color color, bool drawDragPoints)
        {
            Utility.Utility.DrawOutline(sb, Rect, color, 2);

            if (drawDragPoints)
            {
                // upper left
                Utility.Utility.DrawRectangle(sb, new Rectangle(Rect.X, Rect.Y, DRAG_POINT_SIZE, DRAG_POINT_SIZE), color);

                // upper right
                Utility.Utility.DrawRectangle(sb, new Rectangle(Rect.X + Rect.Width - DRAG_POINT_SIZE, Rect.Y, DRAG_POINT_SIZE, DRAG_POINT_SIZE), color);

                // lower left
                Utility.Utility.DrawRectangle(sb, new Rectangle(Rect.X, Rect.Y + Rect.Height - DRAG_POINT_SIZE, DRAG_POINT_SIZE, DRAG_POINT_SIZE), color);

                // lower right
                Utility.Utility.DrawRectangle(sb, new Rectangle(Rect.X + Rect.Width - DRAG_POINT_SIZE, Rect.Y + Rect.Height - DRAG_POINT_SIZE, DRAG_POINT_SIZE, DRAG_POINT_SIZE), color);
            }
        }

        /// <inheritdoc />
        public void HandleDraggingAndResizing(Level level, MouseState lastMouseState, MouseState mouseState)
        {
            Point mousePos = level.Camera.ScreenPosToWorldPos(mouseState.Position.ToVector2()).ToPoint();

            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                // top left
                Point mouseRelative = mousePos - rect.Location;
                if (mouseRelative.X >= 0 && mouseRelative.X <= DRAG_POINT_SIZE
                    && mouseRelative.Y >= 0 && mouseRelative.Y <= DRAG_POINT_SIZE)
                {
                    currentlyDraggingCorner = Corner.TOP_LEFT;
                    return;
                }

                // top right
                mouseRelative = Rect.Location + new Point(Rect.Width, 0) - mousePos;
                if (mouseRelative.X <= DRAG_POINT_SIZE && mouseRelative.X >= 0
                    && mouseRelative.Y <= 0 && mouseRelative.Y >= -DRAG_POINT_SIZE)
                {
                    currentlyDraggingCorner = Corner.TOP_RIGHT;
                    return;
                }

                // bottom left
                mouseRelative = Rect.Location + new Point(0, Rect.Height) - mousePos;
                if (mouseRelative.X <= 0 && mouseRelative.X >= -DRAG_POINT_SIZE
                    && mouseRelative.Y <= DRAG_POINT_SIZE && mouseRelative.Y >= 0)
                {
                    currentlyDraggingCorner = Corner.BOTTOM_LEFT;
                    return;
                }

                // bottom right
                mouseRelative = Rect.Location + Rect.Size - mousePos;
                if (mouseRelative.X <= DRAG_POINT_SIZE && mouseRelative.X >= 0
                    && mouseRelative.Y <= DRAG_POINT_SIZE && mouseRelative.Y >= 0)
                {
                    currentlyDraggingCorner = Corner.BOTTOM_RIGHT;
                    return;
                }

                currentlyDraggingCorner = Corner.NONE;
            }
            else if (lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                Rectangle? newRect = null;
                switch (currentlyDraggingCorner)
                {
                    case Corner.TOP_LEFT:
                        newRect = new Rectangle(mousePos, rect.Size + rect.Location - mousePos);
                        break;

                    case Corner.TOP_RIGHT:
                        Point mouseRelative = mousePos - new Point(rect.X + rect.Width, rect.Y);
                        newRect = new Rectangle(new Point(rect.Location.X, rect.Location.Y + mouseRelative.Y), rect.Size + new Point(mouseRelative.X, -mouseRelative.Y));
                        break;

                    case Corner.BOTTOM_LEFT:
                        mouseRelative = mousePos - new Point(rect.X, rect.Y + rect.Height);
                        newRect = new Rectangle(rect.Location + new Point(mouseRelative.X, 0), rect.Size + new Point(-mouseRelative.X, mouseRelative.Y));
                        break;

                    case Corner.BOTTOM_RIGHT:
                        mouseRelative = mousePos - new Point(rect.X + rect.Width, rect.Y + rect.Height);
                        newRect = new Rectangle(rect.Location, rect.Size + mouseRelative);
                        break;
                }

                
                if (newRect != null)
                {
                    Rect = (Rectangle)newRect;

                    // minimum size check
                    if (Rect.Width <= 20)
                    {
                        rect.Size = new Point(21, rect.Height);
                    }

                    if (Rect.Height <= 20)
                    {
                        rect.Size = new Point(rect.Width, 21);
                    }
                }
            }
            else if (lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                currentlyDraggingCorner = Corner.NONE;
            }
        }

        /// <inheritdoc />
        public XElement ToXml()
        {
            return new XElement("RectCollider",
                Rect.ToXml("Rect"));
        }

        public static RectCollider FromXml(XElement xEle)
        {
            return new RectCollider() {Rect = new Rectangle().FromXml(xEle.Element("Rect"))};
        }

        private enum Corner
        {
            TOP_LEFT,
            TOP_RIGHT,
            BOTTOM_LEFT,
            BOTTOM_RIGHT,
            NONE
        }
    }
}
