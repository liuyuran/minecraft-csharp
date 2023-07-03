using System;
using Base.Components;
using Base.Interface;
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
            player.AddComponent<Health>();
            player.AddComponent<Equipment>();
            player.AddComponent<Storage>();
            LogManager.Instance.Info($"{loginMessage.Nickname} joined the game!");
        }
    }
}