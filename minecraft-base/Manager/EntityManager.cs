using System;
using System.Collections.Generic;
using System.Linq;
using Base.Interface;
using Base.Utils;

namespace Base.Manager {
    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager {
        public static EntityManager Instance { get; } = new();

        private readonly List<Entity> _entities = new();

        public Entity Instantiate() {
            var entity = new Entity(Guid.NewGuid().ToString());
            _entities.Add(entity);
            return entity;
        }
        
        public IEnumerable<Entity> QueryByComponents(params Type[] components) {
            return (from entity in _entities.ToArray() let hasAllComponents = components.All(entity.HasComponent) where hasAllComponents select entity).ToArray();
        }
        
        public void Destroy(Entity entity) {
            _entities.Remove(entity);
        }
        
        public void AddComponent<T>(Entity entity) where T : IComponentData, new() {
            entity.AddComponent<T>();
        }
        
        public void RemoveComponent<T>(Entity entity) where T : IComponentData {
            entity.RemoveComponent<T>();
        }
        
        public bool HasComponent<T>(Entity entity) where T : IComponentData {
            return entity.HasComponent<T>();
        }
        
        public T GetComponent<T>(Entity entity) where T : IComponentData {
            return entity.GetComponent<T>();
        }
    }
}