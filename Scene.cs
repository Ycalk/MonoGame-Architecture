using Architecture.Entities;
using Architecture.Entities.System;
using Architecture.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Intrinsics.X86;

namespace Architecture
{
    public class Scene
    {
        public IEnumerable<IInteractive> Ignoring => ButtonManager.Ignoring().Cast<IInteractive>().Concat(CubeManager.Ignoring());
        public bool ChangingTarget => _changeCameraTargetIgnoringTimer > 0;

        protected Texture2D Background;
        protected readonly ButtonManager ButtonManager;
        protected readonly ImageManager ImageManager;
        protected readonly TextManager TextManager;
        protected readonly CubeManager CubeManager;
        protected readonly GraphicsDevice Graphics;


        protected float RotationMaximalSpeed
        {
            get => _rotationMaximalSpeed;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Rotation speed cannot be negative");
                _rotationMaximalSpeed = value;
            }
        }

        protected float RotationSpeed
        {
            get => _rotationSpeed;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Rotation speed cannot be negative");
                _rotationSpeed = value;
            }
        }

        protected float ZoomMaximalSpeed
        {
            get => _zoomMaximalSpeed;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Zoom speed cannot be negative");
                _zoomMaximalSpeed = value;
            }
        }

        public Vector3 CameraDelta { get; private set; } = Vector3.Zero;

        protected float ZoomSpeed
        {
            get => _zoomSpeed;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Zoom speed cannot be negative");
                _zoomSpeed = value;
            }
        }

        private float _changeCameraTargetIgnoringTimer;


        public const float MinimalCameraDistancing = 50;
        public const float MaximalCameraDistancing = 200;

        private float _rotationMaximalSpeed = 1.5f;
        private float _rotationSpeed = 1.5f;
        private float _rotationCurrentSpeed;

        private bool _leftArrowPress;
        private bool _rightArrowPress;


        private float _zoomMaximalSpeed = 1;
        private float _zoomSpeed = 1.5f;
        private float _zoomCurrentSpeed;

        private bool _upArrowPress;
        private bool _downArrowPress;
        public Scene(IEnumerable<Button> buttons, 
            IEnumerable<Image> images, 
            IEnumerable<Text> texts, 
            IEnumerable<Cube> cubes,
            float distancing,
            GraphicsDevice graphics,
            Texture2D background)
        {
            Graphics = graphics;

            ButtonManager = new ButtonManager(buttons);
            ImageManager = new ImageManager(images);
            TextManager = new TextManager(texts);
            CubeManager = new CubeManager(graphics, cubes, distancing);
            Background = background;
        }

        public Scene(IEnumerable<Button> buttons,
            IEnumerable<Image> images,
            IEnumerable<Text> texts,
            IEnumerable<Cube> cubes,
            GraphicsDevice graphics,
            Texture2D background)
            : this(buttons, images, texts, cubes, 30, graphics, background) {}
            

        public virtual void Update(GameTime gameTime, Screen screen)
        {
            ButtonManager.Manage(gameTime, screen);
            ImageManager.Manage(gameTime, screen);
            TextManager.Manage(gameTime, screen);
            CubeManager.Manage(gameTime, screen);

            RotateCamera(gameTime);
            ZoomCamera(gameTime);

            if (_changeCameraTargetIgnoringTimer > 0)
                _changeCameraTargetIgnoringTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void RotateCamera(GameTime gameTime)
        {
            if (_rightArrowPress || _leftArrowPress)
            {
                var direction = _rightArrowPress ? 1 : -1;
                _rotationCurrentSpeed += direction * _rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (direction * _rotationCurrentSpeed >= _rotationMaximalSpeed)
                    _rotationCurrentSpeed = direction * _rotationMaximalSpeed;
            }
            else
            {
                var direction = _rotationCurrentSpeed > 0 ? -1 : 1;
                _rotationCurrentSpeed += direction * _rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (direction * _rotationCurrentSpeed >= 0)
                    _rotationCurrentSpeed = 0;
            }

            CubeManager.RotateCamera(_rotationCurrentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            _leftArrowPress = false;
            _rightArrowPress = false;
        }

        public void ChangeCameraTarget(Vector3 target)
        {
            if (_changeCameraTargetIgnoringTimer > 0)
                return;
            CameraDelta += target;
            foreach (var cube in CubeManager.Elements)
            {
                var movingTime = 0.03f;
                var point = new Vector3(cube.Position.X - target.X, cube.Position.Y, cube.Position.Z - target.Z);
                cube.MoveTo(point, movingTime, true);
                _changeCameraTargetIgnoringTimer = Math.Max(_changeCameraTargetIgnoringTimer, movingTime * 2 * (point - cube.Position).Length());
            }
        }

        private void ZoomCamera(GameTime gameTime)
        {
            if (_upArrowPress && CubeManager.Camera.CurrentDistancing > MinimalCameraDistancing)
            {
                _zoomCurrentSpeed += -_zoomSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (-_zoomCurrentSpeed >= _zoomMaximalSpeed)
                    _zoomCurrentSpeed = -_zoomMaximalSpeed;
            }
            else if (_downArrowPress && CubeManager.Camera.CurrentDistancing < MaximalCameraDistancing)
            {
                _zoomCurrentSpeed += _zoomSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_zoomCurrentSpeed >= _zoomMaximalSpeed)
                    _zoomCurrentSpeed = _zoomMaximalSpeed;
            }
            else
            {
                var direction = _zoomCurrentSpeed > 0 ? -1 : 1;
                _zoomCurrentSpeed += direction * _zoomSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (direction * _zoomCurrentSpeed >= 0)
                    _zoomCurrentSpeed = 0;
            }

            CubeManager.Camera.ChangeDistancing(_zoomCurrentSpeed);
            _upArrowPress = false;
            _downArrowPress = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Screen screen)
        {
            if (CubeManager.Elements.Count == 0) 
                spriteBatch.Draw(Background, new Rectangle(0, 0, screen.Width, screen.Height), Color.White);
            else
                 Graphics.Clear(Color.White);
            CubeManager.DrawCubes();
            ImageManager.DrawEntities(spriteBatch, screen);
            ButtonManager.DrawEntities(spriteBatch, screen);
            TextManager.DrawEntities(spriteBatch, screen);
        }

        public virtual void Add(Entity entity)
        {
            switch (entity)
            {
                case Button button:
                    ButtonManager.Add(button);
                    break;
                case Image image:
                    ImageManager.Add(image);
                    break;
                case Text text:
                    TextManager.Add(text);
                    break;
                case Cube cube:
                    CubeManager.Add(cube);
                    break;
            }
        }

        public virtual void Remove(Entity entity)
        {
            switch (entity)
            {
                case Button button:
                    ButtonManager.Remove(button);
                    break;
                case Image image:
                    ImageManager.Remove(image);
                    break;
                case Text text:
                    TextManager.Remove(text);
                    break;
                case Cube cube:
                    CubeManager.Remove(cube);
                    break;
            }
        }

        public virtual void Ignore(IInteractive entity)
        {
            switch (entity)
            {
                case Button button:
                    ButtonManager.Ignore(button);
                    break;
                case Cube cube:
                    CubeManager.Ignore(cube);
                    break;
            }
        }

        public virtual void DisableIgnore(IInteractive entity)
        {
            switch (entity)
            {
                case Button button:
                    ButtonManager.DisableIgnore(button);
                    break;
                case Cube cube:
                    CubeManager.DisableIgnore(cube);
                    break;
            }
        }

        public virtual IEnumerable<T> GetEntities<T>()
        {
            var type = typeof(T);
            if (type == typeof(Button))
                return ButtonManager.GetEntities().Cast<T>();
            if (type == typeof(Image))
                return ImageManager.GetEntities().Cast<T>();
            if (type == typeof(Text))
                return TextManager.GetEntities().Cast<T>();
            if (type == typeof(Cube))
                return CubeManager.Elements.Cast<T>();
            throw new ArgumentException("Invalid type");
        }

        public virtual void ButtonPress()
        {
            ButtonManager.OnButtonPress();
            CubeManager.OnButtonPress();
        }

        public virtual void ButtonRelease()
        {
            ButtonManager.OnButtonRelease();
            CubeManager.OnButtonRelease();
        }

        public void LeftArrowPress() => _leftArrowPress = true;
        public void RightArrowPress() => _rightArrowPress = true;
        public void UpArrowPress() => _upArrowPress = true;
        public void DownArrowPress() => _downArrowPress = true;
    }
}
