using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 自动存档系统
    /// </summary>
    public class AutoSaveSystem: ISystem {
        public void OnCreate() {
            //
        }

        public void OnUpdate() {
            ArchiveManager.SaveArchive();
        }
    }
}