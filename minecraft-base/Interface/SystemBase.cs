namespace Base.Interface {
    public abstract class SystemBase {
        public bool Enabled = true;
        public abstract void OnCreate();
        public abstract void OnUpdate();
    }
}