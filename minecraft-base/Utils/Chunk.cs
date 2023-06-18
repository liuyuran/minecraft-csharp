using System;
using System.Numerics;
using Base.Blocks;
using Base.Const;

namespace Base.Utils {
    public sealed class Chunk {
        public readonly IBlock[,,] BlockData = new IBlock[ParamConst.ChunkSize,ParamConst.ChunkSize,ParamConst.ChunkSize];
        public long Version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public Vector3 Position;
        public bool IsEmpty;
        
        public IBlock GetBlock(int x, int y, int z) {
            return BlockData[x, y, z];
        }
    }
}