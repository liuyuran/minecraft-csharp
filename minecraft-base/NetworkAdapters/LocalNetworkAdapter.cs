using System;
using System.Collections.Concurrent;
using Base.Interface;

namespace Base.NetworkAdapters {
    public class LocalNetworkAdapter : INetworkAdapter {
        private readonly string _localUserId = Guid.NewGuid().ToString();
        private readonly ConcurrentQueue<GameEvent> _serverEventQueue = new();
        private readonly ConcurrentQueue<GameEvent> _clientEventQueue = new();

        public void Dispose() {
            _serverEventQueue.Clear();
            _clientEventQueue.Clear();
        }

        public void SendToServer<T>(T message) where T : GameEvent {
            message.UserID = _localUserId;
            _serverEventQueue.Enqueue(message);
        }

        public bool TryGetFromServer(out GameEvent? @event) {
            if (_clientEventQueue.TryDequeue(out var gameEvent)) {
                @event = gameEvent;
                return true;
            }

            @event = default;
            return false;
        }

        public void SendToClient<T>(string uuid, T message) where T : GameEvent {
            _clientEventQueue.Enqueue(message);
        }

        public void SendToClient<T>(T message) where T : GameEvent {
            _clientEventQueue.Enqueue(message);
        }

        public bool TryGetFromClient(out GameEvent? @event) {
            if (_serverEventQueue.TryDequeue(out var gameEvent)) {
                @event = gameEvent;
                return true;
            }

            @event = default;
            return false;
        }
    }
}