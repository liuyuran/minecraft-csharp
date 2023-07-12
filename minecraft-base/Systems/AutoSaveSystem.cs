using System;
using Base.Const;
using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 自动存档系统
    /// </summary>
    public class AutoSaveSystem: SystemBase {
        private long _lastSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        public override void OnCreate() {
        }

        public override void OnUpdate() {
            if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _lastSave < ParamConst.AutoSaveInterval) {
                return;
            }
            ArchiveManager.Instance.SaveArchive();
            _lastSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            LogManager.Instance.Debug("服务器定时存档完成");
        }
    }
}