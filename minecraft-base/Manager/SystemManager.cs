using System.Collections.Generic;
using Base.Interface;
using Base.Systems;

namespace Base.Manager {
    /// <summary>
    /// 系统管理器，用来从代码层面注册并启动各类系统
    /// </summary>
    public static class SystemManager {
        private static readonly List<ISystem> Systems = new();
        
        public static void Initialize() {
            RegisterSystem(new CommandApplySystem());
            RegisterSystem(new ChunkGenerateSystem());
            RegisterSystem(new CheckDeadSystem());
            RegisterSystem(new AnimalGenerateSystem());
            RegisterSystem(new StatusSyncSystem());
            RegisterSystem(new AutoSaveSystem());
        }

        private static void RegisterSystem(ISystem system) {
            system.OnCreate();
            Systems.Add(system);
        }
        
        public static void Update() {
            foreach (var system in Systems) {
                system.OnUpdate();
            }
        }
    }
}