using Base.Interface;

namespace Base.Components {
    /// <summary>
    /// 具备生命值，换而言之理论上可被杀死
    /// </summary>
    public class HealthData: IComponentData {
        public long MaxHealth;
        public long Health;
    }
}