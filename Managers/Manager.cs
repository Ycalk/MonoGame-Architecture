using Architecture.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Managers
{
    public class Manager : IManager
    {
        protected readonly List<IGameEntity> Entities = new();
        protected readonly List<IGameEntity> EntitiesToAdd = new();
        protected readonly List<IGameEntity> EntitiesToRemove = new();


        public void Add(IGameEntity entity) =>
            EntitiesToAdd.Add(
                entity ?? throw new ArgumentNullException(nameof(entity), "Null entity cannot be added"));

        public void Remove(IGameEntity entity) =>
            EntitiesToRemove.Add(
                entity ?? throw new ArgumentNullException(nameof(entity), "Null entity cannot be removed"));

        public virtual void Manage(GameTime gameTime, Screen screen)
        {

            foreach (var entity in Entities)
                entity.Update(gameTime, screen);
            foreach (var entity in EntitiesToAdd)
                Entities.Add(entity);
            foreach (var entity in EntitiesToRemove)
                Entities.Remove(entity);

            EntitiesToAdd.Clear();
            EntitiesToRemove.Clear();
        }

        public void Clear() => EntitiesToRemove.AddRange(Entities);

        public virtual void OnWindowResize(Screen oldScreen, Screen newScreen)
        {
            foreach (var entity in Entities)
                entity.OnWindowResize(oldScreen, newScreen);
        }

        public void DrawEntities(SpriteBatch spriteBatch)
        {
            foreach (var entity in Entities.OrderBy(e => e.DrawOrder))
                entity.Draw(spriteBatch);
        }
    }
}
