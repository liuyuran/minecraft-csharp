using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 自动存档系统
    /// </summary>
    public class AutoSaveSystemBase: SystemBase {
        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            ArchiveManager.SaveArchive();
        }
    }
}