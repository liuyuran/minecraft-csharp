using System.Collections.Generic;
using System.Numerics;
using Base.Components;
using Base.Interface;
using Base.Utils;

namespace Base.Events.ServerEvent {
    public class ChunkUpdateEvent: GameEvent {
        public Chunk? Chunk = null;
        public Dictionary<Vector3, DroppedItem> Items = new();
    }
}