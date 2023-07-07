using Base.Components;
using Base.Interface;
using Base.Manager;

namespace Base.Events.Handler {
    public class UserLogoutEventHandler: IGameEventHandler<PlayerLogoutEvent> {
        public void Run(PlayerLogoutEvent logoutMessage) {
            foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(Player))) {
                var player = entity.GetComponent<Player>();
                if (player.Uuid != logoutMessage.UserID) continue;
                EntityManager.Instance.Destroy(entity);
                PlayerManager.Instance.RemovePlayer(logoutMessage.UserID);
                LogManager.Instance.Debug($"Player {player.NickName} logged out");
                break;
            }
        }
    }
}