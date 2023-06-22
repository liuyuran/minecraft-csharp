using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 自动存档系统
    /// </summary>
    public class AutoSaveSystem: Interface.System {
        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            ArchiveManager.SaveArchive();
        }
    }
}