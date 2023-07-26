using System.Collections.Generic;
using Base.Utils;

namespace Base.Manager {
    public class PlayerManager {
        public static PlayerManager Instance { get; } = new();
        
        private readonly IDictionary<string, Entity> _playerLink = new Dictionary<string, Entity>();
        
        public void AddPlayer(string uuid, Entity player) {
            _playerLink[uuid] = player;
        }
        
        public void RemovePlayer(string uuid) {
            _playerLink.Remove(uuid);
        }
        
        public Entity? GetPlayer(string uuid) {
            return _playerLink.TryGetValue(uuid, out var player) ? player : null;
        }

        public List<Entity> GetAllPlayer() {
            return new List<Entity>(_playerLink.Values);
        }
    }
}