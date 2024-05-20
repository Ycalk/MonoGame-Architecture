using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Entities.System
{
    public abstract class Entity3D : Entity
    {
        public Matrix World { get; protected set; }
        public Model Model { get; protected set; }
        public Color Color { get; protected set; } 
        public Texture2D? Texture { get; protected set; }
        public float Alfa { get; protected set; } = 1f;
        public Vector3 Position => World.Translation;

        protected Entity3D(Matrix world, Model model, Color color)
        {
            World = world;
            Model = model;
            Color = color;
        }

        protected Entity3D(Matrix world, Model model, Texture2D effectTexture)
        {
            World = world;
            Model = model;
            Color = Color.White;
            Texture = effectTexture;
        }

        internal virtual void Update(Screen screen, Camera camera, GameTime gameTime) {}

        internal virtual void Draw(Camera camera)
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (var effect in mesh.Effects.Cast<BasicEffect>())
                {
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = World;
                    effect.DiffuseColor = Color.ToVector3();

                    effect.PreferPerPixelLighting = true;
                    effect.EnableDefaultLighting();
                    
                    effect.Alpha = Alfa;
                    
                    if (Texture is not null)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = Texture;
                    }
                }
                mesh.Draw();
            }
        }
    }
}
