using Base.Messages;
using Base.Utils;

namespace Base.NetworkAdapters {
    public interface INetworkAdapter {
        // public
        public void Close();
        // server side
        public void UpdateChunkForUser(Chunk chunk, string userId);
        public bool TryGetJoinedUser(out CommandMessage<UserLogin> login);
        public bool TryGetDisconnectUser(out CommandMessage<int> user);
        public void Disconnect(string uuid);
        // client side
        public Chunk[] GetChunkForUser();
        public string JoinGame(string nickname);
        public void Disconnect();
    }
}