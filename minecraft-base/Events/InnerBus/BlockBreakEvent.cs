using Base.Blocks;
using Base.Utils;

namespace Base.Events.InnerBus {
    public delegate void BlockBreakEventHandler(BlockBreakEvent @event);
    public struct BlockBreakEvent {
        public string UserId;
        public Block Block;
        public Vector3 BlockPos;
        public Vector3 ChunkPos;
        public int WorldId;
    }
}