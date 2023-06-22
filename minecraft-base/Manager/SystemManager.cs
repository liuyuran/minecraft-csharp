using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Base.Interface;
using Base.Systems;
using Base.Systems.CommandResolvers;

namespace Base.Manager {
    /// <summary>
    /// 系统管理器，用来从代码层面注册并启动各类系统
    /// </summary>
    public static class SystemManager {
        private static readonly List<Interface.System> Systems = new();

        public static void Initialize() {
            var assem = typeof(Interface.System).Assembly;
            var baseType = typeof(Interface.System);
            var types = assem.GetExportedTypes();
            foreach (var type in types) {
                if (type.IsAbstract || type.FullName == null) continue;
                if (!type.IsSubclassOf(baseType)) continue;
                if (assem.CreateInstance(
                        type.FullName, false,
                        BindingFlags.ExactBinding,
                        null, new object[] { }, null, null
                    ) is not Interface.System instance) continue;
                RegisterSystem(instance);
            }
        }

        private static void RegisterSystem(Interface.System system) {
            system.OnCreate();
            Systems.Add(system);
        }

        public static void Update() {
            foreach (var system in Systems.Where(system => system.Enabled)) {
                system.OnUpdate();
            }
        }
    }
}