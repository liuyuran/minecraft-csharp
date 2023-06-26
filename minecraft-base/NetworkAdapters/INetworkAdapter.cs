using Base.Components;
using Base.Messages;
using Base.Utils;

namespace Base.NetworkAdapters {
    public interface INetworkAdapter {
        // public
        public void Close();

        // server side
        public void UpdateChunkForUser(Chunk chunk, string userId); // 向用户发送区块数据
        public bool TryGetJoinedUser(out CommandMessage<UserLogin> login); // 获取用户登录队列
        public bool TryGetDisconnectUser(out CommandMessage<string> user); // 获取用户断开连接队列
        public void Disconnect(string uuid, string reason = ""); // 踢出用户
        public bool TryGetChatMessage(out CommandMessage<string> message); // 获取聊天消息队列
        public void BroadcastChatMessage(string message); // 广播聊天消息
        public void SendChatMessage(string uuid, string message); // 向用户发送聊天消息
        public bool TryGetPlayerInfo(out CommandMessage<PlayerInfo> playerInfo); // 获取玩家信息队列

        // client side
        public Chunk[] GetChunkForUser(); // 获取区块数据
        public string JoinGame(string nickname); // 加入游戏
        public void Disconnect(); // 主动断开连接
        public void SendChatMessage(string message); // 发送聊天消息
        public bool GetShownChatMessage(out CommandMessage<string> message); // 获取聊天消息队列
        public void UpdatePlayerInfo(in Transform transform); // 更新玩家信息
    }
}