using Base.Components;
using Base.Interface;
using Base.Manager;

namespace Base.Events.Handler {
    public class ChatEventHandler: IGameEventHandler<ChatEvent> {
        public void Run(ChatEvent e) {
            var player = PlayerManager.Instance.GetPlayer(e.UserID);
            if (player == null) return;
            var data = player.GetComponent<Player>();
            CommandTransferManager.NetworkAdapter?.SendToClient(new ChatEvent {
                Message = $"[{data.NickName}]: {e.Message}"
            });
        }
    }
}