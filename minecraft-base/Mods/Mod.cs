using System;
using System.IO;
using System.Reflection;
using Base.Exceptions;
using Newtonsoft.Json.Linq;

namespace Base.Mods {
    /// <summary>
    /// mod实体
    /// </summary>
    public class Mod {
        public bool Loaded { get; private set; }
        public string ID { get; private set; }
        public string Name { get; private set; }
        private string DllPath { get; }
        private string EntryPoint { get; }
        private readonly IModEntryPoint _entryPoint;
        
        public Mod(string modPath) {
            try {
                var jsonData = JObject.Parse(File.ReadAllText($"{modPath}/mod.json"));
                ID = jsonData["id"]?.ToString() ?? string.Empty;
                Name = jsonData["name"]?.ToString() ?? string.Empty;
                DllPath = $"{modPath}/{jsonData["dll"]}";
                EntryPoint = jsonData["entrypoint"]?.ToString() ?? string.Empty;
                var dll = Assembly.LoadFrom(DllPath);
                var modType = dll.GetType(EntryPoint);
                var mod = Activator.CreateInstance(modType);
                if (mod is IModEntryPoint modInstance) {
                    _entryPoint = modInstance;
                } else {
                    throw new Exception("入口类未实现IModEntryPoint接口");
                }
            } catch (Exception e) {
                throw new ModLoadException(e.Message);
            }
        }

        public void Load() {
            _entryPoint.OnEnable();
            Loaded = true;
        }
        
        public void UnLoad() {
            _entryPoint.OnDisable();
            Loaded = false;
        }
    }
}