using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 自动存档系统
    /// </summary>
    public class AutoSaveSystemBase: SystemBase {
        public override void OnCreate() {
            Enabled = false;
        }

        public override void OnUpdate() {
            ArchiveManager.Instance.SaveArchive();
        }
    }
}