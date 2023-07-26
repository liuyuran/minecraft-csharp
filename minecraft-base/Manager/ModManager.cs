using System.IO;
using Base.Exceptions;
using Base.Mods;

namespace Base.Manager {
    /// <summary>
    /// 扩展管理器
    /// </summary>
    public class ModManager {
        public static ModManager Instance = new();
        private const string ModPath = "mods";

        public void LoadAllMod() {
            if (!Directory.Exists(ModPath)) {
                Directory.CreateDirectory(ModPath);
            }

            var modFiles = Directory.GetDirectories(ModPath);
            foreach (var modFile in modFiles) {
                try {
                    var mod = new Mod($"{ModPath}/{modFile}");
                    mod.Load();
                } catch (ModLoadException e) {
                    // ignored
                }
            }
        }
    }
}