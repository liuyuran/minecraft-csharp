using System;
using Base.Components;
using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 客户端指令读取系统，前后端交互核心
    /// </summary>
    public class CommandApplySystem: ISystem {
        public void OnCreate() {
            //
        }

        public void OnUpdate() {
            if (CommandTransferManager.NetworkAdapter == null) return;
            while (CommandTransferManager.NetworkAdapter.TryGetJoinedUser(out var loginMessage)) {
                var player = EntityManager.Instantiate();
                player.AddComponent(new Player {
                    Uuid = loginMessage.UserID,
                    LastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    NickName = loginMessage.Message.Nickname
                });
                player.AddComponent<Position>();
            }
            while (CommandTransferManager.NetworkAdapter.TryGetDisconnectUser(out var logoutMessage)) {
                foreach (var entity in EntityManager.QueryByComponents(typeof(Player))) {
                    var player = entity.GetComponent<Player>();
                    if (player.Uuid != logoutMessage.UserID) continue;
                    EntityManager.Destroy(entity);
                    break;
                }
            }
        }
    }
}