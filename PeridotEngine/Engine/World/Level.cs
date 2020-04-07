#nullable enable

using System;
using System.Collections.Generic;
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
        public ObservableRangeCollection<ISolid> Solids { get; }
        /// <summary>
        /// Contains all entities in the level.
        /// </summary>
        public ObservableRangeCollection<IEntity> Entities { get; }
        /// <summary>
        /// Contains all physics objects in the level.
        /// </summary>
        public HashSet<IPhysicsObject> PhysicsObjects { get; set; } = new HashSet<IPhysicsObject>();
        /// <summary>
        /// Contains all the physics colliders in the level.
        /// </summary>
        public HashSet<ICollider> Colliders { get; set; } = new HashSet<ICollider>();

        public string TextureDirectory { get; set; } = null;

        public LazyLoadingTextureDictionary? TextureDictionary { get; set; }

        public bool IsPhysicsEnabled { get; set; } = true;

        public LevelSettings Settings { get; set; } = new LevelSettings();

        public Camera Camera { get; set; } = new Camera();
        public bool CameraShouldFollowPlayer { get; set; } = true;

        public Script? Script { get; set; }

        public event EventHandler<GameTime>? OnUpdate;
        public event EventHandler<SpriteBatch>? OnDraw;

        /// <summary>
        /// Create a new empty level.
        /// </summary>
        public Level()
        {
            this.Solids = new ObservableRangeCollection<ISolid>();
            this.Entities = new ObservableRangeCollection<IEntity>();
            this.Solids.CollectionChanged += OnSolidsChanged;
            this.Entities.CollectionChanged += OnEntitiesChanged;
        }

        /// <summary>
        /// First method of the class to be called. Initializes the level.
        /// </summary>
        public void Initialize()
        {
            foreach (ISolid solid in Solids)
                solid.Initialize(this);

            foreach (IEntity entity in Entities)
                entity.Initialize(this);

            Script?.RunAsync(globals: this);
        }

        /// <summary>
        /// Draws the level and everything in it to the specified SpriteBatch.
        /// </summary>
        /// <param name="sb">The SpriteBatch</param>
        public void Draw(SpriteBatch sb)
        {
            // TODO: Cache this
            List<IWorldObject> combinedObjects = new List<IWorldObject>(Solids.Count + Entities.Count);
            combinedObjects.AddRange(Solids);
            combinedObjects.AddRange(Entities);

            combinedObjects.Sort((x, y) => x.ZIndex.CompareTo(y.ZIndex));

            sb.Begin(transformMatrix: Camera.GetMatrix(), blendState: BlendState.AlphaBlend, rasterizerState: RasterizerState.CullNone);

            foreach (IWorldObject obj in combinedObjects)
            {
                if (obj is IParallaxable parallaxObj && parallaxObj.ParallaxMultiplier != 1.0f)
                {
                    sb.End();
                    sb.Begin(transformMatrix: Camera.GetMatrix(new Vector3(parallaxObj.ParallaxMultiplier, parallaxObj.ParallaxMultiplier, 1)),
                             blendState: BlendState.AlphaBlend);
                    obj.Draw(sb, Camera);
                    sb.End();
                    sb.Begin(transformMatrix: Camera.GetMatrix(), blendState: BlendState.AlphaBlend, rasterizerState: RasterizerState.CullNone);
                }
                else
                {
                    obj.Draw(sb, Camera);
                }
            }

            if (Settings.DrawColliders)
            {
                foreach (ICollider collider in Colliders)
                {
                    collider.Draw(sb, Camera, Color.Green, false);
                }
            }

            sb.End();

            OnDraw?.Invoke(this, sb);
        }

        /// <summary>
        /// Update loop of the level.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {
            foreach (ISolid obj in Solids)
            {
                obj.Update(gameTime);
            }

            foreach (IEntity obj in Entities)
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

            level.TextureDictionary = new LazyLoadingTextureDictionary(level.TextureDirectory);

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

                level.Solids.Add(solid);
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

                level.Entities.Add(entity);
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
                new XElement("Solids",
                    from solid in Solids
                    select solid.ToXml(TextureDictionary)),
                new XElement("Entities",
                    from entity in Entities
                    select entity.ToXml(TextureDictionary)),
                new XElement("Colliders",
                    from collider in Colliders
                    select collider.ToXml())
            );
            root.Save(path);
        }

        private void OnSolidsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void OnEntitiesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PhysicsObjects.Clear();
            foreach (IEntity entity in Entities)
            {
                if (entity is IPhysicsObject physObj)
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
