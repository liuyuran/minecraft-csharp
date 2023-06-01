using Base.Interface;

namespace Base.Components {
    /// <summary>
    /// 具备生命值，换而言之理论上可被杀死
    /// </summary>
    public struct Health: IComponentData {
        public long maxHealth;
        public long health;
    }
}