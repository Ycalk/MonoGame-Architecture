using Architecture.Entities.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Managers.System
{
    public abstract class Manager : IManager
    {
        protected readonly List<Entity2D> Entities = new();
        protected readonly List<Entity2D> EntitiesToAdd = new();
        protected readonly List<Entity2D> EntitiesToRemove = new();

        public IEnumerable<Entity2D> GetEntities() => Entities;

        public void Add(Entity2D entity2D) =>
            EntitiesToAdd.Add(
                entity2D ?? throw new ArgumentNullException(nameof(entity2D), "Null entity2D cannot be added"));

        public void Remove(Entity2D entity2D) =>
            EntitiesToRemove.Add(
                entity2D ?? throw new ArgumentNullException(nameof(entity2D), "Null entity2D cannot be removed"));

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

        public void DrawEntities(SpriteBatch spriteBatch, Screen screen)
        {
            foreach (var entity in Entities.OrderBy(e => e.DrawOrder))
                entity.Draw(spriteBatch, screen);
        }
    }
}
