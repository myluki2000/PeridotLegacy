#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;

namespace PeridotEngine.Engine.Utility
{
    class Quad
    {
        public Vector2 Point1 { get; set; }
        public Vector2 Point2 { get; set; }
        public Vector2 Point3 { get; set; }
        public Vector2 Point4 { get; set; }

        /// <inheritdoc />
        public Quad() { }

        /// <inheritdoc />
        public Quad(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            Point4 = point4;
        }

        public bool Intersects(Rectangle otherRect)
        {
            // performance could maybe be improved by doing a quick check using the surrounding rectangle of the collider first

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

            if (pos > 0 && neg > 0) return false;

            return true;
        }

        public void Draw(SpriteBatch sb, Color color, Camera? camera = null)
        {
            Vector2[] verts =
            {
                Point1,
                Point2,
                Point3,
                Point4,
                Point1
            };

            Utility.DrawLineStrip(sb, verts, color, camera?.GetMatrix() ?? Matrix.Identity);
        }

        public XElement ToXml(string name)
        {
            return new XElement(name,
                Point1.ToXml("P1"),
                Point2.ToXml("P2"),
                Point3.ToXml("P3"),
                Point4.ToXml("P4")
            );
        }

        public static Quad FromXml(XElement xEle)
        {
            return new Quad()
            {
                Point1 = new Vector2().FromXml(xEle.Element("P1")),
                Point2 = new Vector2().FromXml(xEle.Element("P2")),
                Point3 = new Vector2().FromXml(xEle.Element("P3")),
                Point4 = new Vector2().FromXml(xEle.Element("P4"))
            };
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
            float ourMin = ourDots.Min();
            float ourMax = ourDots.Max();

            float[] otherDots =
            {
                Vector2.Dot(otherRect.TopLeft().ToVector2(), n),
                Vector2.Dot(otherRect.TopRight().ToVector2(), n),
                Vector2.Dot(otherRect.BottomLeft().ToVector2(), n),
                Vector2.Dot(otherRect.BottomRight().ToVector2(), n)
            };

            // TODO: Optimize this by getting both min and max in one go
            float otherMin = otherDots.Min();
            float otherMax = otherDots.Max();

            // edge projection is colliding if intervals overlap each other
            return ourMin <= otherMax && otherMin <= ourMax;
        }
    }
}
