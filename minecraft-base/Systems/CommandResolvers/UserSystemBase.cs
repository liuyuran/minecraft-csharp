using System;
using Base.Components;
using Base.Interface;
using Base.Manager;

namespace Base.Systems.CommandResolvers {
    /// <summary>
    /// 客户端指令读取，用来处理玩家的加入和退出
    /// </summary>
    public class UserSystemBase: SystemBase {
        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
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
            while (CommandTransferManager.NetworkAdapter.TryGetPlayerInfo(out var playerInfo)) {
                foreach (var entity in EntityManager.QueryByComponents(typeof(Player))) {
                    var player = entity.GetComponent<Player>();
                    if (player.Uuid != playerInfo.UserID) continue;
                    var position = entity.GetComponent<Position>();
                    position.X = playerInfo.Message.Position.X;
                    position.Y = playerInfo.Message.Position.Y;
                    position.Z = playerInfo.Message.Position.Z;
                    entity.SetComponent(position);
                    break;
                }
            }
        }
    }
}