using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PeridotEngine.Misc;

namespace PeridotEngine.World.Physics.Colliders
{
    class QuadCollider : ICollider
    {
        public Vector2 Point1 { get; set; }
        public Vector2 Point2 { get; set; }
        public Vector2 Point3 { get; set; }
        public Vector2 Point4 { get; set; }

        /// <inheritdoc />
        public bool IsColliding(Rectangle otherRect)
        {
            // note that the first point is the one in the bottom left corner for both our quad and the otherRect

            // start with the edges of our quad

            // check first edge
            Vector2 e12 = (Point2 - Point1);
            if (!IsEdgeProjectionColliding(e12, otherRect)) return false;

            // second edge
            Vector2 e23 = Point3 - Point2;
            if (!IsEdgeProjectionColliding(e23, otherRect)) return false;

            // third edge
            Vector2 e34 = Point4 - Point3;
            if (!IsEdgeProjectionColliding(e34, otherRect)) return false;

            // fourth edge
            Vector2 e41 = Point1 - Point4;
            if (!IsEdgeProjectionColliding(e41, otherRect)) return false;


            // now test using the edges of the "otherRect"
            // we only have to test 2 edges because edges opposite of each other have a vector with
            // the same direction

            // first edge
            Vector2 o12 = (otherRect.BottomRight().ToVector2() - otherRect.BottomLeft().ToVector2());
            if (!IsEdgeProjectionColliding(o12, otherRect)) return false;

            Vector2 o23 = (otherRect.TopRight().ToVector2() - otherRect.BottomRight().ToVector2());
            return IsEdgeProjectionColliding(o23, otherRect);
        }

        private bool IsEdgeProjectionColliding(Vector2 edge, Rectangle otherRect)
        {
            Vector2 n = new Vector2(edge.Y, -edge.X);

            float[] ourDots =
            {
                Vector2.Dot(Point1, n),
                Vector2.Dot(Point2, n),
                Vector2.Dot(Point3, n),
                Vector2.Dot(Point4, n)
            };

            // TODO: Optimize this by getting both min and max in one go
            float ourMin = Enumerable.Min(ourDots);
            float ourMax = Enumerable.Max(ourDots);

            float[] otherDots =
            {
                Vector2.Dot(otherRect.TopLeft().ToVector2(), n),
                Vector2.Dot(otherRect.TopRight().ToVector2(), n),
                Vector2.Dot(otherRect.BottomLeft().ToVector2(), n),
                Vector2.Dot(otherRect.BottomRight().ToVector2(), n)
            };

            // TODO: Optimize this by getting both min and max in one go
            float otherMin = Enumerable.Min(otherDots);
            float otherMax = Enumerable.Max(otherDots);

            // edge projection is colliding if intervals overlap each other
            return ourMin <= otherMax && otherMin <= ourMax;

        }

        /// <inheritdoc />
        public bool Contains(Point point)
        {
            int pos = 0;
            int neg = 0;

            float c = CrossProductOfEdgeAndPoint(Point1, Point2, point);
            if (c < 0) neg++;
            if (c > 0) pos++;

            if (pos > 0 && neg > 0) return false;

            c = CrossProductOfEdgeAndPoint(Point2, Point3, point);
            if (c < 0) neg++;
            if (c > 0) pos++;

            if (pos > 0 && neg > 0) return false;

            c = CrossProductOfEdgeAndPoint(Point3, Point4, point);
            if (c < 0) neg++;
            if (c > 0) pos++;

            if (pos > 0 && neg > 0) return false;

            c = CrossProductOfEdgeAndPoint(Point4, Point1, point);
            if (c < 0) neg++;
            if (c > 0) pos++;

            return !(pos <= 0 && neg <= 0);
        }

        /// <summary>
        /// Calculates the cross product between the specified edge and point.
        /// </summary>
        /// <param name="e1">Start point of the edge</param>
        /// <param name="e2">End point of the edge</param>
        /// <param name="p">The point to check</param>
        /// <returns></returns>
        private float CrossProductOfEdgeAndPoint(Vector2 e1, Vector2 e2, Point p)
        {
            return (p.X - e1.X) * (e2.Y - e1.Y) - (p.Y - e1.Y) * (e2.X - e1.X);
        }

        /// <inheritdoc />
        public void Draw(SpriteBatch sb, Color color, bool drawDragPoints)
        {
            Vector2[] verts =
            {
                Point1,
                Point2,
                Point3,
                Point4
            };
            Utility.DrawLineStrip(sb, verts, color);
        }

        /// <inheritdoc />
        public void HandleDraggingAndResizing(Level level, MouseState lastMouseState, MouseState mouseState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public XElement ToXml()
        {
            return new XElement("QuadCollider",
                new XElement("P1", Point1.ToXml()),
                new XElement("P2", Point2.ToXml()),
                new XElement("P3", Point3.ToXml()),
                new XElement("P4", Point4.ToXml())
            );
        }

        public static QuadCollider FromXml(XElement xEle)
        {
            return new QuadCollider()
            {
                Point1 = new Vector2().FromXml(xEle.Element("P1")),
                Point2 = new Vector2().FromXml(xEle.Element("P2")),
                Point3 = new Vector2().FromXml(xEle.Element("P3")),
                Point4 = new Vector2().FromXml(xEle.Element("P4")),
            };
        }
    }
}
