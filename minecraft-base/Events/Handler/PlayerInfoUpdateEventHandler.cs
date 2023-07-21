using System;
using Base.Components;
using Base.Events.ClientEvent;
using Base.Interface;
using Base.Manager;

namespace Base.Events.Handler {
    public class PlayerInfoUpdateEventHandler: IGameEventHandler<PlayerInfoUpdateEvent> {
        public void Run(PlayerInfoUpdateEvent playerInfo) {
            var player = PlayerManager.Instance.GetPlayer(playerInfo.UserID);
            if (player == null) return;
            var transform = player.GetComponent<Transform>();
            transform.Position = playerInfo.Position;
            transform.Forward = playerInfo.Forward;
            player.GetComponent<Player>().LastControlTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}