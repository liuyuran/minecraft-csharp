using Base.Events.ClientEvent;
using Base.Interface;
using Base.Manager;

namespace Base.Events.Handler {
    public class PlayerJoinEventHandler: IGameEventHandler<PlayerJoinEvent> {
        public void Run(PlayerJoinEvent loginMessage) {
            if (PlayerManager.Instance.GetPlayer(loginMessage.UserID) != null) return;
            var player = ArchiveManager.Instance.LoadPlayer(loginMessage.UserID, loginMessage.Nickname);
            PlayerManager.Instance.AddPlayer(loginMessage.UserID, player);
            LogManager.Instance.Info($"{loginMessage.Nickname} joined the game!");
        }
    }
}