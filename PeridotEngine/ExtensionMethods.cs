using System.Globalization;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace PeridotEngine
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Maps this float value in a range to a float value in another range.
        /// </summary>
        public static float Map(this float value, float inputFrom, float inputTo, float outputFrom, float outputTo)
        {
            return (value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
        }

        /// <summary>
        /// Maps this sbyte value in a range to a float value in another range.
        /// </summary>
        public static float Map(this sbyte value, float inputFrom, float inputTo, float outputFrom, float outputTo)
        {
            return (value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
        }

        /// <summary>
        /// Transforms this Vector2 using the specified transformation matrix.
        /// </summary>
        public static Vector2 Transform(this Vector2 value, Matrix transformationMatrix)
        {
            return Vector2.Transform(value, transformationMatrix);
        }

        /// <summary>
        /// Transforms this Vector3 using the specified transformation matrix.
        /// </summary>
        public static Vector3 Transform(this Vector3 value, Matrix transformationMatrix)
        {
            return Vector3.Transform(value, transformationMatrix);
        }

        public static Matrix Invert(this Matrix value)
        {
            return Matrix.Invert(value);
        }

        public static XElement[] ToXml(this Vector2 value)
        {
            return new XElement[]
            {
                new XElement("X", value.X.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new XElement("Y", value.Y.ToString(System.Globalization.CultureInfo.InvariantCulture)),
            };
        }

        public static Vector2 FromXml(this Vector2 value, XElement xEle)
        {
            value.X = float.Parse(xEle.Element("X").Value, CultureInfo.InvariantCulture.NumberFormat);
            value.Y = float.Parse(xEle.Element("Y").Value, CultureInfo.InvariantCulture.NumberFormat);

            return value;
        }
    }
}