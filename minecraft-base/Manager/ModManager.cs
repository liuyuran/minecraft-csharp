using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Base.Exceptions;
using Base.Mods;

namespace Base.Manager {
    /// <summary>
    /// 扩展管理器
    /// </summary>
    [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class ModManager {
        public static readonly ModManager Instance = new();
        private const string ModPath = "mods";
        
        public readonly IDictionary<string, Mod> ModList = new Dictionary<string, Mod>();

        public void ScanAllMod() {
            if (!Directory.Exists(ModPath)) {
                Directory.CreateDirectory(ModPath);
            }

            var modFiles = Directory.GetDirectories(ModPath);
            foreach (var modFile in modFiles) {
                try {
                    var mod = new Mod($"{ModPath}/{modFile}");
                    ModList.Add(mod.ID, mod);
                } catch (ModLoadException e) {
                    // ignored
                }
            }
        }
        
        public void LoadAllMod() {
            foreach (var mod in ModList.Values) {
                mod.Load();
            }
        }
        
        public void UnLoadAllMod() {
            foreach (var mod in ModList.Values) {
                mod.UnLoad();
            }
        }
        
        public Mod? GetMod(string name) {
            return ModList.TryGetValue(name, out var value) ? value : null;
        }
        
        public void LoadMod(string name) {
            if (ModList.TryGetValue(name, out var value)) {
                value.Load();
            }
        }
        
        public void UnLoadMod(string name) {
            if (ModList.TryGetValue(name, out var value)) {
                value.UnLoad();
            }
        }
    }
}