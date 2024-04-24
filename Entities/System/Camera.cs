using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Entities.System
{
    public enum CameraStartPosition
    {
        Twenty = 20,
        Thirty = 30,
        Forty = 40,
        Fifty = 50,
        Sixty = 60,
        Seventy = 70,
        Eighty = 80,
    }

    internal class Camera
    {
        public CameraStartPosition StartPosition { get; }
        public Vector3 Position { get; private set; }
        public readonly Vector3 Target;
        public Matrix View { get; private set; }
        public readonly Matrix Projection;

        public Camera(float aspectRatio, CameraStartPosition startPositionX, float angel, Vector3 target)
        {
            Target = target;
            StartPosition = startPositionX;

            Projection = Matrix.CreatePerspectiveFieldOfView(
                angel, aspectRatio, 1, 1000);
            Position = new Vector3((int)startPositionX, 0, 0);
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

        public void Draw(IEnumerable<_3DEntity> entities)
        {
            foreach (var obj in entities)
                DrawObject(obj);
        }

        private void DrawObject(_3DEntity obj)
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
