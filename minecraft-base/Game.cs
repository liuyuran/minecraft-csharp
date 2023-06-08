using System;
using System.Threading;
using Base.Manager;
using Base.NetworkAdapters;

namespace Base {
    public static class Game {
        public static void Start(string path) {
            Start(path, new LocalNetworkAdapter());
        }
        
        public static void Start(string path, INetworkAdapter adapter) {
            // 设定网络模式
            CommandTransferManager.Init(adapter);
            // 扫描存档，如果存档存在，读取，如果不存在，初始化
            ArchiveManager.LoadArchive(path);
            // 初始化系统
            SystemManager.Initialize();
            // 开始游戏循环
            var prev = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            while (true) {
                // 确保最高100ms一次的刷新频率
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (now - prev < 100) {
                    Thread.Sleep(10);
                    continue;
                }
                SystemManager.Update();
                prev = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}