namespace Base.Interface {
    public abstract class System {
        public bool Enabled = true;
        public abstract void OnCreate();
        public abstract void OnUpdate();
    }
}