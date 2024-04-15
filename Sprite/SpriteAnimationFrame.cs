namespace Architecture.Sprite
{
    public class SpriteAnimationFrame
    {
        public float TimeStamp { get; }
        public Sprite Sprite
        {
            get => _sprite;
            set => _sprite = value ?? throw new ArgumentNullException(nameof(value), "Sprite cannot be null");
        }

        private Sprite _sprite;

        public SpriteAnimationFrame(Sprite sprite, float timeStamp)
        {
            _sprite = sprite;
            TimeStamp = timeStamp;
        }
    }
}
