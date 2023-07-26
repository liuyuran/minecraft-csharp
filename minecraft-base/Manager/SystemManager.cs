using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Base.Interface;

namespace Base.Manager {
    /// <summary>
    /// 系统管理器，用来从代码层面注册并启动各类系统
    /// </summary>
    public static class SystemManager {
        private static readonly List<SystemBase> Systems = new();

        public static void Initialize() {
            var assem = typeof(SystemBase).Assembly;
            var baseType = typeof(SystemBase);
            var types = assem.GetExportedTypes();
            foreach (var type in types) {
                if (type.IsAbstract || type.FullName == null) continue;
                if (!type.IsSubclassOf(baseType)) continue;
                if (assem.CreateInstance(
                        type.FullName, false,
                        BindingFlags.ExactBinding,
                        null, new object[] { }, null, null
                    ) is not SystemBase instance) continue;
                RegisterSystem(instance);
            }
        }

        private static void RegisterSystem(SystemBase systemBase) {
            systemBase.OnCreate();
            Systems.Add(systemBase);
        }

        public static void Update() {
            var systems = Systems.Where(system => system.Enabled).ToArray();
            foreach (var system in systems) {
                system.OnUpdate();
            }
        }
    }
}