using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 用于将客户端消息转换为游戏事件的系统
    /// </summary>
    public class ClientMessageSystem : SystemBase {
        public override void OnCreate() {
            MessageTypeManager.Instance.GameEventHandlers += MessageHandler;
        }

        private static void MessageHandler(object sender, GameEvent e) {
            MessageTypeManager.Instance.FireEvent(e);
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