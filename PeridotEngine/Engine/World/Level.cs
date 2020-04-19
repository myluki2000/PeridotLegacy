#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PeridotEngine.Engine.Graphics;
using PeridotEngine.Engine.Graphics.Effects;
using PeridotEngine.Engine.Resources;
using PeridotEngine.Engine.World.Physics;
using PeridotEngine.Engine.World.Physics.Colliders;
using PeridotEngine.Engine.World.WorldObjects;
using PeridotEngine.Engine.World.WorldObjects.Entities;
using PeridotEngine.Engine.World.WorldObjects.Solids;
using PeridotEngine.Engine.Utility;

namespace PeridotEngine.Engine.World
{
    public class Level
    {
        /// <summary>
        /// Contains all WorldObjects placed in the level.
        /// </summary>
        public ObservableRangeCollection<IWorldObject> WorldObjects { get; private set; }
        /// <summary>
        /// Contains all physics objects in the level.
        /// </summary>
        public HashSet<IPhysicsObject> PhysicsObjects { get; set; } = new HashSet<IPhysicsObject>();
        /// <summary>
        /// Contains all the physics colliders in the level.
        /// </summary>
        public HashSet<ICollider> Colliders { get; set; } = new HashSet<ICollider>();

        public string TextureDirectory { get; set; } = null;

        public LazyLoadingMaterialDictionary? TextureDictionary { get; set; }

        public bool IsPhysicsEnabled { get; set; } = true;

        public LevelSettings Settings { get; set; } = new LevelSettings();

        public Camera Camera { get; set; } = new Camera();
        public bool CameraShouldFollowPlayer { get; set; } = true;
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;

        public Script? Script { get; set; }

        public event EventHandler<GameTime>? OnUpdate;
        public event EventHandler<SpriteBatch>? OnDraw;

        /// <summary>
        /// Create a new empty level.
        /// </summary>
        public Level()
        {
            this.WorldObjects = new ObservableRangeCollection<IWorldObject>();
            this.WorldObjects.CollectionChanged += OnWorldObjectsChanged;
        }

        /// <summary>
        /// First method of the class to be called. Initializes the level.
        /// </summary>
        public void Initialize()
        {
            foreach (IWorldObject obj in WorldObjects)
                obj.Initialize(this);

            Script?.RunAsync(globals: this);
        }

        /// <summary>
        /// Draws the level and everything in it to the specified SpriteBatch.
        /// </summary>
        /// <param name="sb">The SpriteBatch</param>
        public void Draw(SpriteBatch sb)
        {
            float lastParallaxValue = float.NaN;
            foreach (IWorldObject obj in WorldObjects)
            {
                // skip objects that don't have anything to draw
                if (!(obj is IRenderedObject rObj)) continue;

                float parallaxValue = (obj is IParallaxable pObj) ? pObj.ParallaxMultiplier : 1.0f;

                if (parallaxValue != lastParallaxValue || rObj.DisableBatching)
                {
                    // if it isn't the first time we begin a new SpriteBatch we'll first have to end the old one
                    if (!float.IsNaN(lastParallaxValue)) sb.End();

                    Matrix transformMatrix = parallaxValue != 1.0f ? Camera.GetMatrix(parallaxValue) : Camera.GetMatrix();

                    sb.Begin(blendState: BlendState.AlphaBlend,
                        rasterizerState: RasterizerState.CullNone,
                        transformMatrix: transformMatrix);

                }

                rObj.Draw(sb, Camera);
                lastParallaxValue = parallaxValue;
            }

            sb.End();



            if (Settings.DrawColliders)
            {
                sb.Begin(transformMatrix: Camera.GetMatrix());
                foreach (ICollider collider in Colliders)
                {
                    collider.Draw(sb, Camera, Color.Green, false);
                }
                sb.End();
            }



            OnDraw?.Invoke(this, sb);
        }

        public void DrawGlowMap(SpriteBatch sb)
        {
            float lastParallaxValue = float.NaN;
            foreach (IWorldObject obj in WorldObjects)
            {
                if (!(obj is IRenderedObject rObj)) continue;

                float parallaxValue = (obj is IParallaxable pObj) ? pObj.ParallaxMultiplier : 1.0f;

                if (parallaxValue != lastParallaxValue || rObj.DisableBatching)
                {
                    // if it isn't the first time we begin a new SpriteBatch we'll first have to end the old one
                    if (!float.IsNaN(lastParallaxValue)) sb.End();

                    Matrix transformMatrix = parallaxValue != 1.0f ? Camera.GetMatrix(parallaxValue) : Camera.GetMatrix();

                    sb.Begin(blendState: BlendState.AlphaBlend,
                        rasterizerState: RasterizerState.CullNone,
                        transformMatrix: transformMatrix);

                }

                rObj.DrawGlowMap(sb, Camera);
                lastParallaxValue = parallaxValue;
            }

            sb.End();
        }

        /// <summary>
        /// Update loop of the level.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {
            foreach (IWorldObject obj in WorldObjects)
            {
                obj.Update(gameTime);

                if (CameraShouldFollowPlayer && obj is Player)
                {
                    Camera.FocusOnPosition(obj.Position + obj.Size / 2);
                }
            }

            if (IsPhysicsEnabled)
            {
                PhysicsHelper.UpdatePhysics(this, gameTime);
            }

            OnUpdate?.Invoke(this, gameTime);
        }

        public IWorldObject GetObjectById(string id)
        {
            return WorldObjects.FirstOrDefault(x => x.Id == id);
        }

        public IWorldObject[] GetObjectsByClass(string _class)
        {
            return WorldObjects.Where(x => x.Class != null && x.Class.Split(' ').Contains(_class)).ToArray();
        }

        public static Level FromFile(string path)
        {
            Level level = new Level();

            string scriptPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".csx");
            if (File.Exists(scriptPath))
            {
                // get GameTime's assembly (MonoGame.Framework) to load that assembly, because we need it for almost everything
                ScriptOptions scriptOptions = ScriptOptions.Default.AddReferences(typeof(GameTime).Assembly);
                level.Script = CSharpScript.Create(
                    File.ReadAllText(scriptPath),
                    globalsType: typeof(Level),
                    options: scriptOptions);

                level.Script.Compile();

            }

            XElement rootEle = XElement.Load(path);

            level.TextureDirectory = Path.Combine(Path.GetDirectoryName(path), rootEle.Element("TextureDirectory").Value);
            level.TextureDictionary = new LazyLoadingMaterialDictionary(level.TextureDirectory);

            if(rootEle.Element("BackgroundColor") != null)
                level.BackgroundColor = new Color().FromXml(rootEle.Element("BackgroundColor"));

            // loop through all solids, find their type with reflection, create a new instance of that type
            // and let it initialize itself with the provided xml.
            foreach (XElement xEle in rootEle.Element("Solids").Elements())
            {
                Type solidType = Type.GetType("PeridotEngine.Engine.World.WorldObjects.Solids." + xEle.Name.LocalName);

                // if solid is not implemented by engine check if it's implemented by the game
                if (solidType == null)
                {
                    solidType = Type.GetType("PeridotEngine.Game.World.WorldObjects.Solids." + xEle.Name.LocalName);
                }

                ISolid solid = (ISolid)solidType.GetMethod("FromXml").Invoke(null, new object[] { xEle, level.TextureDictionary });

                level.WorldObjects.Add(solid);
            }

            // do the same for entities
            foreach (XElement xEle in rootEle.Element("Entities").Elements())
            {
                Type entityType = Type.GetType("PeridotEngine.Engine.World.WorldObjects.Entities." + xEle.Name.LocalName);

                // if entity is not implemented by engine check if it's implemented by the game
                if (entityType == null)
                {
                    entityType = Type.GetType("PeridotEngine.Game.World.WorldObjects.Entities." + xEle.Name.LocalName);
                }

                IEntity entity = (IEntity)entityType.GetMethod("FromXml").Invoke(null, new object[] { xEle, level.TextureDictionary });

                level.WorldObjects.Add(entity);
            }

            // do the same for colliders
            foreach (XElement xEle in rootEle.Element("Colliders").Elements())
            {
                Type colliderType = Type.GetType("PeridotEngine.Engine.World.Physics.Colliders." + xEle.Name.LocalName);

                // if collider is not implemented by engine check if it's implemented by the game
                if (colliderType == null)
                {
                    colliderType = Type.GetType("PeridotEngine.Game.World.Physics.Colliders." + xEle.Name.LocalName);
                }

                ICollider collider = (ICollider)colliderType.GetMethod("FromXml").Invoke(null, new object[] { xEle });

                level.Colliders.Add(collider);
            }

            return level;
        }

        public void ToFile(string path)
        {
            XElement root = new XElement("Level",
                new XElement("TextureDirectory", TextureDirectory.Substring(TextureDirectory.IndexOf(@"\", StringComparison.InvariantCulture) + 1)), // remove the leading "world\" from the path
                BackgroundColor.ToXml("BackgroundColor"),
                new XElement("Solids",
                    from solid in WorldObjects.Where(x => x is ISolid)
                    select solid.ToXml(TextureDictionary)),
                new XElement("Entities",
                    from entity in WorldObjects.Where(x => x is IEntity)
                    select entity.ToXml(TextureDictionary)),
                new XElement("Colliders",
                    from collider in Colliders
                    select collider.ToXml())
            );
            root.Save(path);
        }

        private void OnWorldObjectsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            WorldObjects = new ObservableRangeCollection<IWorldObject>(
                WorldObjects.OrderBy(x => x.ZIndex)
                    .ThenBy(x => (x is IParallaxable p) ? p.ParallaxMultiplier : 1.0f)
                    .ThenBy(x => (x is ITextured t) ? t.Material.Diffuse.Texture.GetHashCode() : 0));
            WorldObjects.CollectionChanged += OnWorldObjectsChanged;

            // update the PhysicsObjects list with all physics objects.
            PhysicsObjects.Clear();
            foreach (IWorldObject wObj in WorldObjects)
            {
                if (wObj is IPhysicsObject physObj)
                {
                    PhysicsObjects.Add(physObj);
                }
            }
        }

        public class LevelSettings
        {
            public bool DrawColliders { get; set; } = true;
        }
    }
}
