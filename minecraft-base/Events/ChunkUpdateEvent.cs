using Base.Interface;
using Base.Utils;

namespace Base.Events {
    public class ChunkUpdateEvent: GameEvent {
        public Chunk? Chunk = null;
    }
}