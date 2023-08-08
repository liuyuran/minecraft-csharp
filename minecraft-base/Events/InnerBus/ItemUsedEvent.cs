using Base.Blocks;
using Base.Items;
using Base.Utils;

namespace Base.Events.InnerBus {
    public delegate void ItemUsedEventHandler(ItemUsedEvent @event);
    public struct ItemUsedEvent {
        public string UserId;
        public Block Block;
        public Vector3 BlockPos;
        public Vector3 ChunkPos;
        public int WorldId;
        public Direction Direction;
        public Item Item;
    }
}