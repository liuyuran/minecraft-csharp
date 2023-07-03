using System;
using Base.Components;
using Base.Interface;
using Base.Manager;
using Base.Messages;

namespace Base.MessageHandlers {
    public class PlayerJoinEventHandler: IGameEventHandler<PlayerJoinEvent> {
        public void Run(PlayerJoinEvent loginMessage) {
            Console.WriteLine(loginMessage.Nickname + " joined the game!");
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
        }
    }
}