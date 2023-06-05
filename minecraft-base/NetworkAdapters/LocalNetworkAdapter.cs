using System.Collections.Concurrent;
using System.Collections.Generic;
using Base.Messages;
using Base.Utils;

namespace Base.NetworkAdapters {
    public class LocalNetworkAdapter : INetworkAdapter {
        private readonly ConcurrentQueue<CommandMessage<Chunk>> _chunkOutQueue = new();
        private readonly ConcurrentQueue<CommandMessage<UserLogin>> _loginInQueue = new();
        private readonly ConcurrentQueue<CommandMessage<int>> _logoutInQueue = new();

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

        public bool TryGetDisconnectUser(out CommandMessage<int> user) {
            if (!_logoutInQueue.IsEmpty) return _logoutInQueue.TryDequeue(out user);
            user = default;
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
            var uuid = System.Guid.NewGuid().ToString();
            _loginInQueue.Enqueue(new CommandMessage<UserLogin> {
                UserID = uuid,
                Message = new UserLogin {
                    Nickname = nickname
                }
            });
            return uuid;
        }

        public void Disconnect() {
            var uuid = System.Guid.NewGuid().ToString();
            _logoutInQueue.Enqueue(new CommandMessage<int> {
                UserID = uuid,
                Message = 0
            });
        }
    }
}