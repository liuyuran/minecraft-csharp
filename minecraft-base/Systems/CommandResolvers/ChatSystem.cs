using Base.Interface;
using Base.Manager;

namespace Base.Systems.CommandResolvers {
    /// <summary>
    /// 客户端指令读取，用来处理玩家的聊天指令
    /// </summary>
    public class ChatSystem: Interface.System {
        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            if (CommandTransferManager.NetworkAdapter == null) return;
            while (CommandTransferManager.NetworkAdapter.TryGetChatMessage(out var message)) {
                // TODO 也许会有指令之类的场景
                CommandTransferManager.NetworkAdapter.BroadcastChatMessage($"{message.UserID}:{message.Message}");
            }
        }
    }
}