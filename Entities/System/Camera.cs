using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities.System
{
    internal class Camera
    {
        public Vector3 Position { get; private set; }
        public readonly Vector3 Target;
        public Matrix View { get; private set; }
        public readonly Matrix Projection;

        public Camera(float aspectRatio, float distancing, float angel, Vector3 target)
        {
            Target = target;

            Projection = Matrix.CreatePerspectiveFieldOfView(
                angel, aspectRatio, 1, 1000);

            Position = new Vector3(distancing, 0, 0);
            var rotationMatrix = Matrix.CreateRotationZ(angel);
            Position = Vector3.Transform(Position, rotationMatrix);

            View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        }

        public void Rotate(float angleInRadians)
        {
            var rotationMatrix = Matrix.CreateRotationY(angleInRadians);
            Position = Vector3.Transform(Position, rotationMatrix);
            View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        }

        public void Draw(IEnumerable<Entity3D> entities)
        {
            foreach (var obj in entities)
                DrawObject(obj);
        }

        private void DrawObject(Entity3D obj)
        {
            foreach (var mesh in obj.Model.Meshes)
            {
                foreach (var effect in mesh.Effects.Cast<BasicEffect>())
                {
                    effect.View = View;
                    effect.Projection = Projection;
                    effect.World = obj.World;
                }
                mesh.Draw();
            }
        }
    }
}
