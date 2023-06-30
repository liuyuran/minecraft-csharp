using Base.NetworkAdapters;

namespace Base.Manager {
    /// <summary>
    /// 作为状态同步和客户端命令输入的适配中转
    /// </summary>
    public static class CommandTransferManager {
        public static INetworkAdapter? NetworkAdapter;
        
        /// <summary>
        /// 加载适配器
        /// </summary>
        /// <param name="adapter">自行初始化的适配器</param>
        public static void Init(INetworkAdapter adapter) {
            Close();
            NetworkAdapter = adapter;
        }

        private static void Close() {
            NetworkAdapter?.Dispose();
            NetworkAdapter = null;
        }
    }
}