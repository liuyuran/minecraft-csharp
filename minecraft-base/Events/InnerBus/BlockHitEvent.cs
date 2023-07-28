﻿using System.Numerics;
using Base.Blocks;

namespace Base.Events.InnerBus {
    public delegate void BlockHitEventHandler(BlockHitEvent @event);
    public struct BlockHitEvent {
        public string UserId;
        public Block Block;
        public Vector3 BlockPos;
        public Vector3 ChunkPos;
        public long WorldId;
    }
}