using System;
using Base.Components;
using Base.Interface;
using Base.Items;
using Base.Manager;

namespace Base.Events.Handler {
    public class PlayerJoinEventHandler: IGameEventHandler<PlayerJoinEvent> {
        public void Run(PlayerJoinEvent loginMessage) {
            var player = EntityManager.Instance.Instantiate();
            player.AddComponent(new Player {
                Uuid = loginMessage.UserID,
                LastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                NickName = loginMessage.Nickname
            });
            player.AddComponent<Transform>();
            player.AddComponent<World>();
            player.AddComponent<HealthData>();
            player.AddComponent<Equipment>();
            player.AddComponent(new Inventory {
                Size = 32,
                Items = new Item[32]
            });
            player.AddComponent<ToolInHand>();
            PlayerManager.Instance.AddPlayer(loginMessage.UserID, player);
            LogManager.Instance.Info($"{loginMessage.Nickname} joined the game!");
        }
    }
}