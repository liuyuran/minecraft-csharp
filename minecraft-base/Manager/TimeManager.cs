namespace Base.Manager {
    /// <summary>
    /// 游戏内时间管理器
    /// </summary>
    public class TimeManager {
        public static TimeManager Instance { get; } = new();
        public long Tick { get;private set; }

        public void Update() {
            Tick++;
        }
        
        public void Reset() {
            Tick = 0;
        }
        
        public void SetTick(long tick) {
            Tick = tick;
        }
    }
}