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
    }
}