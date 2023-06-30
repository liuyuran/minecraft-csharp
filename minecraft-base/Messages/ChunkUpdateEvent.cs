using Base.Interface;
using Base.Utils;

namespace Base.Messages {
    public class ChunkUpdateEvent: GameEvent {
        public Chunk Chunk;
    }
}