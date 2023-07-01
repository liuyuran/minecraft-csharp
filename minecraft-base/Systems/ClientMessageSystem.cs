using System;
using Base.Components;
using Base.Interface;
using Base.Manager;
using Base.Messages;

namespace Base.Systems {
    /// <summary>
    /// 集中处理所有来自客户端的指令
    /// </summary>
    public class ClientMessageSystem : SystemBase {
        public override void OnCreate() {
            MessageTypeManager.Instance.GameEventHandlers += UserMessage;
            MessageTypeManager.Instance.GameEventHandlers += ChatMessage;
        }

        private static void ChatMessage(object sender, GameEvent e) { }

        private static void UserMessage(object sender, GameEvent e) {
            switch (e) {
                case PlayerInfoUpdateEvent playerInfo: {
                    foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(Player))) {
                        var player = entity.GetComponent<Player>();
                        if (player.Uuid != playerInfo.UserID) continue;
                        var transform = entity.GetComponent<Transform>();
                        transform.Position = playerInfo.Position;
                        transform.Forward = playerInfo.Forward;
                        entity.SetComponent(transform);
                        break;
                    }

                    break;
                }
                case PlayerJoinEvent loginMessage: {
                    var player = EntityManager.Instance.Instantiate();
                    player.AddComponent(new Player {
                        Uuid = loginMessage.UserID,
                        LastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        NickName = loginMessage.Nickname
                    });
                    player.AddComponent<Transform>();
                    player.AddComponent<World>();
                    player.AddComponent<Health>();
                    player.AddComponent<Equipment>();
                    player.AddComponent<Storage>();
                    break;
                }
                case PlayerLogoutEvent logoutMessage: {
                    foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(Player))) {
                        var player = entity.GetComponent<Player>();
                        if (player.Uuid != logoutMessage.UserID) continue;
                        EntityManager.Instance.Destroy(entity);
                        break;
                    }

                    break;
                }
            }
        }

        public override void OnUpdate() {
            if (CommandTransferManager.NetworkAdapter == null) return;
            while (CommandTransferManager.NetworkAdapter.TryGetFromClient(out var message)) {
                if (message == null) return;
                MessageTypeManager.Instance.OnGameEventHandlers(message);
            }
        }
    }
}