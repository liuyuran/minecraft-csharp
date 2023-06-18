using Base.Const;

namespace Base.Manager {
    /// <summary>
    /// 存档管理器
    /// </summary>
    public static class ArchiveManager {
        private static string _archiveName = "default";

        public static void LoadArchive(string path) {
            _archiveName = ParamConst.ArchiveBasePath + "/" + _archiveName;
        }

        public static void SaveArchive() {
            //
        }
    }
}