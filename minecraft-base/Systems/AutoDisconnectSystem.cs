using System;
using Base.Components;
using Base.Const;
using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 长时间无交互自动踢人系统
    /// </summary>
    public class AutoDisconnectSystem: SystemBase {
        public override void OnCreate() {
        }

        public override void OnUpdate() {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(Player))) {
                var lastSync = entity.GetComponent<Player>().LastSyncTime;
                if (now - lastSync < ParamConst.DisconnectTimeout) continue;
                var player = entity.GetComponent<Player>();
                var uuid = player.Uuid;
                var nickname = player.NickName;
                PlayerManager.Instance.RemovePlayer(uuid);
                LogManager.Instance.Warning($"玩家 {nickname} 由于长时间无交互被踢出游戏");
            }
        }
    }
}