using Base.Component;

namespace Base.Entity {
    public interface IEntity {
        string ID { get; }
        IComponent AddComponent<T>() where T: IComponent;
        IComponent AddComponent(IComponent component);
        IComponent GetComponent<T>() where T: IComponent;
        IComponent GetComponent(string? name);
    }
}