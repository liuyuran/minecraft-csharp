using Base.Components;
using Base.Interface;
using Base.Manager;

namespace Base.Events.Handler {
    public class UserLogoutEventHandler: IGameEventHandler<PlayerLogoutEvent> {
        public void Run(PlayerLogoutEvent logoutMessage) {
            var player = PlayerManager.Instance.GetPlayer(logoutMessage.UserID);
            if (player == null) return;
            var playerData = player.GetComponent<Player>();
            EntityManager.Instance.Destroy(player);
            PlayerManager.Instance.RemovePlayer(logoutMessage.UserID);
            LogManager.Instance.Debug($"Player {playerData.NickName} logged out");
        }
    }
}