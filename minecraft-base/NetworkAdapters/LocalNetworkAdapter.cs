using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using Base.Components;
using Base.Messages;
using Base.Utils;

namespace Base.NetworkAdapters {
    public class LocalNetworkAdapter : INetworkAdapter {
        private readonly string _localUserId = System.Guid.NewGuid().ToString();
        private readonly ConcurrentQueue<CommandMessage<Chunk>> _chunkOutQueue = new();
        private readonly ConcurrentQueue<CommandMessage<UserLogin>> _loginInQueue = new();
        private readonly ConcurrentQueue<CommandMessage<string>> _logoutInQueue = new();
        private readonly ConcurrentQueue<CommandMessage<string>> _chatQueue = new();
        private readonly ConcurrentQueue<CommandMessage<PlayerInfo>> _playerInfoQueue = new();

        public void Close() {
            _chunkOutQueue.Clear();
            _loginInQueue.Clear();
            _logoutInQueue.Clear();
        }

        public bool TryGetJoinedUser(out CommandMessage<UserLogin> login) {
            if (!_loginInQueue.IsEmpty) return _loginInQueue.TryDequeue(out login);
            login = default;
            return false;
        }

        public bool TryGetDisconnectUser(out CommandMessage<string> user) {
            if (!_logoutInQueue.IsEmpty) return _logoutInQueue.TryDequeue(out user);
            user = default;
            return false;
        }

        public bool TryGetChatMessage(out CommandMessage<string> message) {
            if (!_chatQueue.IsEmpty) return _chatQueue.TryDequeue(out message);
            message = default;
            return false;
        }

        public void BroadcastChatMessage(string message) {
            throw new System.NotImplementedException();
        }

        public void SendChatMessage(string uuid, string message) {
            throw new System.NotImplementedException();
        }

        public bool TryGetPlayerInfo(out CommandMessage<PlayerInfo> playerInfo) {
            if (!_playerInfoQueue.IsEmpty) return _playerInfoQueue.TryDequeue(out playerInfo);
            playerInfo = default;
            return false;
        }

        public Chunk[] GetChunkForUser() {
            var chunks = new List<Chunk>();
            while (!_chunkOutQueue.IsEmpty) {
                _chunkOutQueue.TryDequeue(out var chunk);
                chunks.Add(chunk.Message);
            }
            return chunks.ToArray();
        }

        public void UpdateChunkForUser(Chunk chunk, string userId) {
            _chunkOutQueue.Enqueue(new CommandMessage<Chunk> {
                UserID = userId,
                Message = chunk
            });
        }

        public string JoinGame(string nickname) {
            _loginInQueue.Enqueue(new CommandMessage<UserLogin> {
                UserID = _localUserId,
                Message = new UserLogin {
                    Nickname = nickname
                }
            });
            return _localUserId;
        }

        public void Disconnect() {
            _logoutInQueue.Enqueue(new CommandMessage<string> {
                UserID = _localUserId,
                Message = ""
            });
        }

        public void SendChatMessage(string message) {
            _chatQueue.Enqueue(new CommandMessage<string> {
                UserID = _localUserId,
                Message = message
            });
        }

        public bool GetShownChatMessage(out CommandMessage<string> message) {
            throw new System.NotImplementedException();
        }

        public void UpdatePlayerInfo(in Transform transform) {
            _playerInfoQueue.Enqueue(new CommandMessage<PlayerInfo> {
                UserID = _localUserId,
                Message = new PlayerInfo {
                    Transform = transform.Position,
                    Forward = transform.Forward
                }
            });
        }

        public void Disconnect(string uuid, string reason) {
            _logoutInQueue.Enqueue(new CommandMessage<string> {
                UserID = uuid,
                Message = reason
            });
        }
    }
}