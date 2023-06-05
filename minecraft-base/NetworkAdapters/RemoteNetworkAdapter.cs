using Base.Messages;
using Base.Utils;

namespace Base.NetworkAdapters {
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