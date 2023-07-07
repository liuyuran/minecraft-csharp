using System;
using Base.Interface;

namespace Base.NetworkAdapters {
    public interface INetworkAdapter: IDisposable {
        public string GetCurrentPlayerUuid(); // 获取当前玩家的 UUID
        public void SendToServer<T>(T message) where T: GameEvent; // 向服务器发送消息
        public bool TryGetFromServer(out GameEvent? @event); // 从服务器获取消息
        public void SendToClient<T>(string uuid, T message) where T: GameEvent; // 向客户端发送消息
        public void SendToClient<T>(T message) where T: GameEvent; // 向客户端广播消息
        public bool TryGetFromClient(out GameEvent? @event); // 从客户端获取消息
    }
}