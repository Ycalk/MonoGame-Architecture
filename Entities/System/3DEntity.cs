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
        protected Entity3D(Matrix world, Model model, Color color)
        {
            World = world;
            Model = model;
            Color = color;
        }

        internal virtual void Update(Screen screen, Camera camera) {}

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
                }
                mesh.Draw();
            }
        }
    }
}
