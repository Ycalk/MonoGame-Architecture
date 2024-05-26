using Architecture.Entities.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Architecture.Entities
{
    public class Cube : Entity3D, IInteractive
    {
        protected static readonly Vector3[] VerticesInLocalCoordinates = {
            new(-1, 1, 1),
            new(-1, 1, -1),
            new(1, 1, -1),
            new(1, 1, 1),
        };

        protected Color HoveringColor = Color.Gray;
        protected Color PressingColor = Color.Red;

        public bool IsPressed { get; internal set; }
        public bool IsHovered { get; internal set; }

        private bool _hovering;
        private bool _pressing;

        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private float _currentMovingTime;
        private float _totalMovingTime;
        private bool _isMoving;

        private bool _isDisappearing;
        private float _disappearingTime;

        private Vector2 ConvertIntoTwoDimensions(Vector3 vector,
            Matrix view, Matrix projection,
            int windowWidth, int windowHeight)
        {
            var vertexInWorldCoordinates = Vector3.Transform(vector, World);

            var projected = Vector3.Transform(vertexInWorldCoordinates, view * projection);

            var screenPosition = new Vector2(
                projected.X / projected.Z,
                projected.Y / projected.Z
            );

            screenPosition.X = (screenPosition.X + 1) * windowWidth / 2;
            screenPosition.Y = (1 - screenPosition.Y) * windowHeight / 2;
            return screenPosition;
        }

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

        public void SetAlfa(float alfa)
        {
            if (alfa is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(alfa), "Alfa must be between 0 and 1");
            Alfa = alfa;
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
                    VerticesInLocalCoordinates[i],
                    camera.View, camera.Projection,
                    screen.Width, screen.Height);
            }
            return CheckIntersection(points);
        }

        internal override void Update(Screen screen, Camera camera, GameTime gameTime)
        {
            if (_isMoving)
            {
                _currentMovingTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_currentMovingTime >= _totalMovingTime)
                {
                    World = Matrix.CreateWorld(_endPosition, Vector3.Forward, Vector3.Up);
                    _isMoving = false;
                }
                else
                {
                    var newPosition = Vector3.Lerp(_startPosition,
                        _endPosition, _currentMovingTime / _totalMovingTime);
                    World = Matrix.CreateWorld(newPosition, Vector3.Forward, Vector3.Up);
                }
            }

            if (_isDisappearing)
            {
                Alfa -= (float)gameTime.ElapsedGameTime.TotalSeconds / _disappearingTime;
                if (Alfa <= 0)
                {
                    Color = Color.Transparent;
                    _isDisappearing = false;
                }
            }

            InteractiveCheck(screen, camera);
        }

        private void InteractiveCheck(Screen screen, Camera camera)
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
                    if (CheckIntersection(screen, camera))
                        OnRelease();
                    _pressing = false;
                }
                if (IsHovered || !_hovering) return;
                OnLeave();
                _hovering = false;
            }
        }

        public Cube(Vector3 position, Model model, Texture2D texture)
            : base(Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up), model, texture) {} 

        public Cube(Vector3 position, Model model)
            : base(Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up), model, Color.White) {}

        public void Disappear(float disappearingTime)
        {
            _isDisappearing = true;
            _disappearingTime = disappearingTime;
        }

        public void MoveTo(Vector3 position, float movingTime)
        {
            _startPosition = World.Translation;
            _endPosition = position;
            _totalMovingTime = movingTime;
            _currentMovingTime = 0;
            _isMoving = true;
        }

        public void MoveTo(Vector3 position) =>
            World = Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up);

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            var other = (Cube)obj;
            return other.World.Translation == World.Translation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(World.Translation);
        }
    }
}
