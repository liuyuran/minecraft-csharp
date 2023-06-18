﻿using Base.Messages;
using Base.NetworkAdapters;
using Base.Utils;

namespace Server {
    /// <summary>
    /// 远程网络适配器，用于构建C/S架构的独立服务器
    /// 在此模式下，仅需实现服务端的相关接口
    /// 客户端也需要实现一个与其对应的类
    /// </summary>
    public class RemoteNetworkAdapter: INetworkAdapter {
        public void Close() {
            throw new System.NotImplementedException();
        }

        public void UpdateChunkForUser(Chunk chunk, string userId) {
            throw new System.NotImplementedException();
        }

        public bool TryGetJoinedUser(out CommandMessage<UserLogin> login) {
            throw new System.NotImplementedException();
        }

        public bool TryGetDisconnectUser(out CommandMessage<int> user) {
            throw new System.NotImplementedException();
        }

        public void Disconnect(string uuid) {
            throw new NotImplementedException();
        }

        public Chunk[] GetChunkForUser() {
            throw new System.NotImplementedException();
        }

        public string JoinGame(string nickname) {
            throw new System.NotImplementedException();
        }

        public void Disconnect() {
            throw new System.NotImplementedException();
        }
    }
}