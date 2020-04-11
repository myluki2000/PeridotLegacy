#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PeridotEngine.Engine.Resources
{
    /// <summary>
    /// A dictionary for textures, which will automatically load a texture if it is not already in the dictionary.
    /// </summary>
    public class LazyLoadingMaterialDictionary : Dictionary<string, Material>
    {
        public new Material this[string key]
        {
            get
            {
                if (!base.ContainsKey(key))
                {
                    base.Add(key, TextureManager.LoadMaterial(Path.Combine(TextureDirectory, key)));
                }

                if(base.ContainsKey(key))
                {
                    return base[key];
                } 
                else
                {
                    throw new Exception("Error while lazy loading textures.");
                }
            }
        }

        public LazyLoadingMaterialDictionary(string textureDirectory)
        {
            this.TextureDirectory = textureDirectory;
        }

        public string GetTexturePathByName(string textureName)
        {
            return this.FirstOrDefault(x => x.Value.Name == textureName).Key;
        }

        public string TextureDirectory { get; set; }
    }
}
