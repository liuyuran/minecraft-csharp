using System;
using System.Collections.Generic;
using Base.Interface;

namespace Base.Utils {
    /// <summary>
    /// 实体，除了ID什么都不存，就是一个组合主体
    /// </summary>
    public class Entity {
        public readonly string ID;
        private readonly Dictionary<string, IComponentData> _componentMap = new();

        public Entity(string id) {
            ID = id;
        }
        
        public void AddComponent<T>() where T : IComponentData, new() {
            var name = typeof(T).Name;
            if (_componentMap.ContainsKey(name)) return;
            _componentMap.Add(name, new T());
        }
        
        public void AddComponent(IComponentData component) {
            _componentMap.Add(component.GetType().Name, component);
        }
        
        public void RemoveComponent<T>() where T : IComponentData {
            _componentMap.Remove(typeof(T).Name);
        }
        
        public bool HasComponent<T>() where T : IComponentData {
            return _componentMap.ContainsKey(typeof(T).Name);
        }
        
        public bool HasComponent(Type type) {
            return _componentMap.ContainsKey(type.Name);
        }
        
        public T GetComponent<T>() where T : IComponentData {
            return (T) _componentMap[typeof(T).Name];
        }
    }
}