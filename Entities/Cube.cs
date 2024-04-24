using Architecture.Entities.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Architecture.Entities
{
    public class Cube : _3DEntity
    {
        protected static readonly Vector3[] ProjectedVertices = {
            new(-1, 2, 1),
            new(-1, 2, -1),
            new(1, 2, -1),
            new(1, 2, 1),
        };

        protected Color HoveringColor = Color.Gray;
        protected Color PressingColor = Color.Red;

        public bool IsPressed { get; internal set; }
        public bool IsHovered { get; internal set; }

        private bool _hovering;
        private bool _pressing;


        // Magic starts here
        private readonly Dictionary<CameraStartPosition, float> _offsets = new()
        {
            [CameraStartPosition.Twenty] = 42,
            [CameraStartPosition.Thirty] = 22,
            [CameraStartPosition.Forty] = 15,
            [CameraStartPosition.Fifty] = 11,
            [CameraStartPosition.Sixty] = 9,
            [CameraStartPosition.Seventy] = 9,
            [CameraStartPosition.Eighty] = 7,
        };

        private Vector2 ConvertIntoTwoDimensions(Vector3 vector,
            Matrix view, Matrix projection, Vector3 cameraPosition, CameraStartPosition position,
            int windowWidth, int windowHeight)
        {
            var projected = Vector3.Transform(vector, view * projection);
            var windowScale = (float)Math.Sqrt(
                cameraPosition.X * cameraPosition.X +
                cameraPosition.Y * cameraPosition.Y +
                cameraPosition.Z * cameraPosition.Z) * 2;
            return new Vector2(
                projected.X * windowWidth / windowScale +
                (float)windowWidth / 2 - 2,

                -projected.Y * windowHeight / windowScale +
                (float)windowHeight / 2 + _offsets[position]);
        }
        // Magic ends here

        public virtual void OnHover()
        {
            Color = HoveringColor;
        }

        public virtual void OnLeave()
        {
            Color = Color.White;
        }

        public virtual void OnPress()
        {
            Color = PressingColor;
        }

        public virtual void OnRelease()
        {
            Color = IsHovered ? HoveringColor : Color.White;
        }

        private static bool CheckIntersection(Vector2[] points)
        {
            var minX = points.Min(p => p.X);
            var maxX = points.Max(p => p.X);
            var minY = points.Min(p => p.Y);
            var maxY = points.Max(p => p.Y);
            var mouse = Mouse.GetState();
            var mouseX = mouse.X;
            var mouseY = mouse.Y;
            return mouseX >= minX && mouseX <= maxX && mouseY >= minY && mouseY <= maxY;
        }

        internal bool CheckIntersection(Screen screen, Camera camera)
        {
            var points = new Vector2[4];
            for (var i = 0; i < 4; i++)
            {
                points[i] = ConvertIntoTwoDimensions(
                    ProjectedVertices[i] + World.Translation,
                    camera.View, camera.Projection, camera.Position, camera.StartPosition,
                    screen.Width, screen.Height);
            }
            return CheckIntersection(points);
        }

        internal override void Update(Screen screen, Camera camera)
        {
            if (IsPressed && !_pressing)
            {
                OnPress();
                _pressing = true;
            }
            else if (IsHovered && !_hovering)
            {
                OnHover();
                _hovering = true;
            }
            else
            {
                if (!IsPressed && _pressing)
                {
                    OnRelease();
                    _pressing = false;
                }
                if (IsHovered || !_hovering) return;
                OnLeave();
                _hovering = false;
            }
        }

        public Cube(Vector3 position, Model model)
            : base(Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up), model, Color.White) {}

        public void MoveTo(Vector3 position) =>
            World = Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up);
    }
}
