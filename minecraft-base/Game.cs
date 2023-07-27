using System;
using System.Threading;
using Base.Manager;
using Base.NetworkAdapters;

namespace Base {
    /// <summary>
    /// 游戏服务器核心逻辑循环入口类
    /// </summary>
    public static class Game {
        private static bool _isRunning; 
        
        /// <summary>
        /// 以本地适配器模式启动服务器
        /// </summary>
        /// <param name="path">存档地址</param>
        public static void Start(string path) {
            Start(path, new LocalNetworkAdapter());
        }

        public static void Stop() {
            _isRunning = false;
        }
        
        /// <summary>
        /// 以自定义适配器模式启动服务器
        /// </summary>
        /// <param name="path">存档地址</param>
        /// <param name="adapter">自定义适配器实例</param>
        // ReSharper disable once MemberCanBePrivate.Global
        public static void Start(string path, INetworkAdapter adapter) {
            _isRunning = true;
            LogManager.Instance.Info("服务器启动中，请稍等...");
            // 初始化mod管理器
            ModManager.Instance.ScanAllMod();
            LogManager.Instance.Info("扩展模组扫描完成");
            // 设定网络模式
            CommandTransferManager.Init(adapter);
            LogManager.Instance.Info("初始化网络适配器完成");
            // 扫描存档，如果存档存在，读取，如果不存在，初始化
            ArchiveManager.Instance.LoadArchive(path);
            LogManager.Instance.Info("读取存档完成");
            // 初始化系统
            SystemManager.Initialize();
            LogManager.Instance.Info("加载逻辑系统完成");
            // 开始游戏循环
            var prev = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            LogManager.Instance.Info("逻辑服务器启动完毕");
            while (_isRunning) {
                // 确保最高100ms一次的刷新频率
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (now - prev < 100) {
                    Thread.Sleep(10);
                    continue;
                }
                SystemManager.Update();
                prev = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
    }
}