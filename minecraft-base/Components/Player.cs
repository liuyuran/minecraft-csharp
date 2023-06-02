using Base.Interface;

namespace Base.Components {
    /// <summary>
    /// 基础玩家特性
    /// </summary>
    public class Player: IComponentData {
        /// <summary>
        /// 通信唯一标识符
        /// </summary>
        public string Uuid;
        /// <summary>
        /// 最后同步时间
        /// </summary>
        public long LastSyncTime;
        /// <summary>
        /// 角色显示名称
        /// </summary>
        public string NickName;
    }
}