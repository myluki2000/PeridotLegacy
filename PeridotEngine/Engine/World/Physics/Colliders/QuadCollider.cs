#nullable enable

using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.World.Physics.Colliders
{
    class QuadCollider : ICollider
    {
        public Quad Quad { get; set; } = new Quad();

        private const int DRAG_POINT_SIZE = 10;

        private Corner currentlyDraggingCorner = Corner.NONE;

        /// <inheritdoc />
        public bool IsColliding(Rectangle otherRect)
        {
            return Quad.Intersects(otherRect);
        }

        /// <inheritdoc />
        public bool Contains(Point point)
        {
            return Quad.Contains(point);
        }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Camera camera, Color color, bool drawDragPoints)
        {
            Vector2[] verts =
            {
                Quad.Point1,
                Quad.Point2,
                Quad.Point3,
                Quad.Point4,
                Quad.Point1
            };

            Utility.Utility.DrawLineStrip(sb, verts, color, camera.GetMatrix());

            if (drawDragPoints)
            {
                // upper left
                Utility.Utility.DrawRectangle(sb, new Rectangle((int)Quad.Point4.X, (int)Quad.Point4.Y, DRAG_POINT_SIZE, DRAG_POINT_SIZE), color);

                // upper right
                Utility.Utility.DrawRectangle(sb, new Rectangle((int)Quad.Point3.X - DRAG_POINT_SIZE, (int)Quad.Point3.Y, DRAG_POINT_SIZE, DRAG_POINT_SIZE), color);

                // lower left
                Utility.Utility.DrawRectangle(sb, new Rectangle((int)Quad.Point1.X, (int)Quad.Point1.Y - DRAG_POINT_SIZE, DRAG_POINT_SIZE, DRAG_POINT_SIZE), color);

                // lower right
                Utility.Utility.DrawRectangle(sb, new Rectangle((int)Quad.Point2.X - DRAG_POINT_SIZE, (int)Quad.Point2.Y - DRAG_POINT_SIZE, DRAG_POINT_SIZE, DRAG_POINT_SIZE), color);
            }
        }

        /// <inheritdoc />
        public void HandleDraggingAndResizing(Level level, MouseState lastMouseState, MouseState mouseState)
        {
            Point mousePos = level.Camera.ScreenPosToWorldPos(mouseState.Position.ToVector2()).ToPoint();

            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                // top left
                Point mouseRelative = mousePos - Quad.Point4.ToPoint();
                if (mouseRelative.X >= 0 && mouseRelative.X <= DRAG_POINT_SIZE
                    && mouseRelative.Y >= 0 && mouseRelative.Y <= DRAG_POINT_SIZE)
                {
                    currentlyDraggingCorner = Corner.TOP_LEFT;
                    return;
                }

                // top right
                mouseRelative = Quad.Point3.ToPoint() - mousePos;
                if (mouseRelative.X <= DRAG_POINT_SIZE && mouseRelative.X >= 0
                    && mouseRelative.Y <= 0 && mouseRelative.Y >= -DRAG_POINT_SIZE)
                {
                    currentlyDraggingCorner = Corner.TOP_RIGHT;
                    return;
                }

                // bottom left
                mouseRelative = Quad.Point1.ToPoint() - mousePos;
                if (mouseRelative.X <= 0 && mouseRelative.X >= -DRAG_POINT_SIZE
                    && mouseRelative.Y <= DRAG_POINT_SIZE && mouseRelative.Y >= 0)
                {
                    currentlyDraggingCorner = Corner.BOTTOM_LEFT;
                    return;
                }

                // bottom right
                mouseRelative = Quad.Point2.ToPoint() - mousePos;
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
                switch (currentlyDraggingCorner)
                {
                    case Corner.TOP_LEFT:
                        Quad.Point4 = mousePos.ToVector2();
                        break;

                    case Corner.TOP_RIGHT:
                        Quad.Point3 = mousePos.ToVector2();
                        break;

                    case Corner.BOTTOM_LEFT:
                        Quad.Point1 = mousePos.ToVector2();
                        break;

                    case Corner.BOTTOM_RIGHT:
                        Quad.Point2 = mousePos.ToVector2();
                        break;
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
            return Quad.ToXml("QuadCollider");
        }

        public static QuadCollider FromXml(XElement xEle)
        {
            return new QuadCollider
            {
                Quad = Quad.FromXml(xEle)
            };
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
