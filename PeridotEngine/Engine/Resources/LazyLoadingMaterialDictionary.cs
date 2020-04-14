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
                    LoadMaterial(key);
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

        /// <summary>
        /// Loads the material from the specified path to the dictionary. Does nothing if material from same path is already loaded.
        /// </summary>
        /// <param name="path"></param>
        public void LoadMaterial(string path)
        {
            if (path.StartsWith(TextureDirectory))
            {
                path = path.Substring(TextureDirectory.Length + 1);
            }

            if (ContainsKey(path)) return;

            base.Add(path, TextureManager.LoadMaterial(Path.Combine(TextureDirectory, path)));
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
