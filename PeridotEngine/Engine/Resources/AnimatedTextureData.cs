using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace PeridotEngine.Engine.Resources
{
    public sealed class AnimatedTextureData : TextureDataBase
    {
        public int FrameCount { get; }
        public int[] FrameDurations { get; }

        public override int Width
        {
            get => width ?? Texture.Width / FrameCount;
            set => width = value;
        }

        public override int Height
        {
            get => height ?? Texture.Height;
            set => height = value;
        }

        private int? width = null;
        private int? height = null;

        public AnimatedTextureData(string name, Texture2D texture, bool hasRandomTextureRotation, int frameCount, int[] frameDurations)
        {
            this.Name = name;
            this.Texture = texture;
            this.HasRandomTextureRotation = hasRandomTextureRotation;
            this.FrameCount = frameCount;
            this.FrameDurations = frameDurations;
        }

        public static AnimatedTextureData FromXml(XElement xEle)
        {
            string randomTextureRotString = xEle.Element("RandomTextureRotation").Value.ToUpper();
            bool randomTextureRot;

            // check and throw exception in case the file is broken
            switch (randomTextureRotString)
            {
                case "TRUE":
                    randomTextureRot = true;
                    break;
                case "FALSE":
                    randomTextureRot = false;
                    break;
                default:
                    throw new System.Exception("Error while parsing texture data: Invalid xml value for random texture rotation for texture " + xEle.Element("Name").Value);
            }

            int frameCount = int.Parse(xEle.Element("FrameCount").Value);
            

            string durString = xEle.Element("FrameDurations").Value;
            int[] frameDurations = durString.Split(',').Select(x => int.Parse(x.Trim())).ToArray();

            AnimatedTextureData tex = new AnimatedTextureData(
                xEle.Element("Name").Value,
                TextureManager.LoadRawTexture(xEle.Element("ImagePath").Value),
                randomTextureRot,
                frameCount,
                frameDurations
            );

            // set texture size if set in .ptex file
            XElement xEleWidth = xEle.Element("Width");
            if (xEleWidth != null)
            {
                tex.Width = int.Parse(xEleWidth.Value);
            }

            XElement xEleHeight = xEle.Element("Height");
            if (xEleHeight != null)
            {
                tex.Height = int.Parse(xEleHeight.Value);
            }

            return tex;
        }
    }
}
