using Base.Components;
using Base.Interface;
using Base.Manager;
using Base.Messages;

namespace Base.MessageHandlers {
    public class PlayerInfoUpdateEventHandler: IGameEventHandler<PlayerInfoUpdateEvent> {
        public void Run(PlayerInfoUpdateEvent playerInfo) {
            foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(Player))) {
                var player = entity.GetComponent<Player>();
                if (player.Uuid != playerInfo.UserID) continue;
                var transform = entity.GetComponent<Transform>();
                transform.Position = playerInfo.Position;
                transform.Forward = playerInfo.Forward;
                entity.SetComponent(transform);
                break;
            }
        }
    }
}