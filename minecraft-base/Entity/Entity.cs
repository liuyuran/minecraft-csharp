using System;
using System.Collections.Generic;
using Base.Component;

namespace Base.Entity {
    public class Entity: IEntity {
        private string _id = "";
        public virtual string ID => _id;
        private readonly IDictionary<string, IComponent> _components = new Dictionary<string, IComponent>();
        
        public void SetID(string id) {
            _id = id;
        }
        
        private IComponent AddComponent(Type type) {
            var component = (IComponent?) Activator.CreateInstance(type);
            if (component == null || type.FullName == null) {
                throw new Exception("创建Component失败");
            }
            _components.Add(type.FullName, component);
            return component;
        }

        public IComponent AddComponent<T>() where T : IComponent {
            return AddComponent(typeof(T));
        }

        public IComponent AddComponent(IComponent component) {
            return AddComponent(component.GetType());
        }

        public IComponent GetComponent<T>() where T : IComponent {
            var name = typeof(T).FullName;
            return GetComponent(name);
        }

        public IComponent GetComponent(string? name) {
            if (name == null || !_components.ContainsKey(name)) {
                throw new Exception($"Component {name} 不存在");
            }
            return _components[name];
        }
    }
}