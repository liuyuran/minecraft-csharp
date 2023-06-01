using System;
using System.Collections.Generic;
using System.Linq;
using Base.Interface;
using Base.Utils;

namespace Base.Manager {
    /// <summary>
    /// 实体管理器
    /// </summary>
    public static class EntityManager {
        private static readonly List<Entity> Entities = new();

        public static Entity Instantiate() {
            var entity = new Entity(Guid.NewGuid().ToString());
            Entities.Add(entity);
            return entity;
        }
        
        public static Entity[] QueryByComponents(params Type[] components) {
            return (from entity in Entities let hasAllComponents = components.All(entity.HasComponent) where hasAllComponents select entity).ToArray();
        }
        
        public static void Destroy(Entity entity) {
            Entities.Remove(entity);
        }
        
        public static void AddComponent<T>(Entity entity) where T : IComponentData, new() {
            entity.AddComponent<T>();
        }
        
        public static void RemoveComponent<T>(Entity entity) where T : IComponentData {
            entity.RemoveComponent<T>();
        }
        
        public static bool HasComponent<T>(Entity entity) where T : IComponentData {
            return entity.HasComponent<T>();
        }
        
        public static T GetComponent<T>(Entity entity) where T : IComponentData {
            return entity.GetComponent<T>();
        }
    }
}