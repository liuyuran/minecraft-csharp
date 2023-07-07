namespace Base.Interface {
    public interface IGameEventHandler<in T> where T: GameEvent {
        public void Run(T e);
    }
}