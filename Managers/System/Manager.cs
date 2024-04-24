using Architecture.Entities.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Managers.System
{
    public abstract class Manager : IManager
    {
        protected readonly List<Entity> Entities = new();
        protected readonly List<Entity> EntitiesToAdd = new();
        protected readonly List<Entity> EntitiesToRemove = new();


        public void Add(Entity entity) =>
            EntitiesToAdd.Add(
                entity ?? throw new ArgumentNullException(nameof(entity), "Null entity cannot be added"));

        public void Remove(Entity entity) =>
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

        public void DrawEntities(SpriteBatch spriteBatch, Screen screen)
        {
            foreach (var entity in Entities.OrderBy(e => e.DrawOrder))
                entity.Draw(spriteBatch, screen);
        }
    }
}
