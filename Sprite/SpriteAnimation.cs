using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Sprite
{
    public class SpriteAnimation
    {
        public bool IsPlaying { get; private set; }
        public float PlaybackProgress { get; private set; }
        public readonly bool ShouldLoop;

        public SpriteAnimationFrame this[int index] => GetFrame(index);
        public float Duration =>
            _frames.Count == 0 ? 0 : _frames.Max(f => f.TimeStamp);
        public SpriteAnimationFrame? CurrentFrame =>
            _frames
                .Where(f => f.TimeStamp <= PlaybackProgress)
                .MaxBy(f => f.TimeStamp);

        private readonly List<SpriteAnimationFrame> _frames = new();

        public SpriteAnimation(bool shouldLoop = true) =>
            ShouldLoop = shouldLoop;
        
        
        public void AddFrame(Sprite sprite, float timeStamp) =>
            _frames.Add(new SpriteAnimationFrame(sprite, timeStamp));

        public void Update(GameTime gameTime)
        {
            if (!IsPlaying) return;

            PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (PlaybackProgress < Duration) return;

            if (ShouldLoop)
                PlaybackProgress -= Duration;
            else
                Stop();
        }

        public void Draw(SpriteBatch spriteBatch, Point position) =>
            CurrentFrame?.Sprite.Draw(spriteBatch, position);

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            PlaybackProgress = 0;
        }

        public SpriteAnimationFrame GetFrame(int index)
        {
            if (index < 0 || index >= _frames.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the frames list");
            return _frames[index];
        }

        public void Clear()
        {
            Stop();
            _frames.Clear();
        }
    }
}
