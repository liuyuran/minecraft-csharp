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
            while (CommandTransferManager.LoginInQueue.TryDequeue(out var loginMessage)) {
                var player = EntityManager.Instantiate();
                player.AddComponent(new Player {
                    Uuid = loginMessage.UserID,
                    LastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    NickName = loginMessage.Message.Nickname
                });
            }
            while (CommandTransferManager.LogoutInQueue.TryDequeue(out var logoutMessage)) {
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