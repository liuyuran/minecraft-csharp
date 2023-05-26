using System.Collections.Generic;
using Base.Component;
using Base.Entity;
using Base.Util;

namespace Base.Game {
    public class Game {
        private readonly List<IEntity> _entities = new();

        public Game() {
            var entity = CreateEntity();
            entity.AddComponent<Transform>();
            entity.AddComponent<Rotation>();
        }

        private IEntity CreateEntity() {
            IEntity entity = new Entity.Entity();
            _entities.Add(entity);
            return entity;
        }
        
        public IEntity[] GetEntities(Matcher matcher) {
            return matcher.Match(_entities);
        }
    }
}