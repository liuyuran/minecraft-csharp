using System;
using System.Numerics;
using Base.Blocks;
using Base.Const;

namespace Base.Utils {
    public sealed class Chunk {
        // 六个面的可见性掩码
        public const int Left = 0b1;
        public const int Right = 0b10;
        public const int Up = 0b100;
        public const int Down = 0b1000;
        public const int Front = 0b10000;
        public const int Back = 0b100000;

        public readonly Block[,,] BlockData = new Block[ParamConst.ChunkSize,ParamConst.ChunkSize,ParamConst.ChunkSize];
        public long Version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public Vector3 Position;
        public bool IsEmpty;
        
        public Block GetBlock(int x, int y, int z) {
            return BlockData[x, y, z];
        }
        
        public void SetBlock(int x, int y, int z, Block block) {
            var r = 0;
            if (x < 1 || GetBlock(x - 1, y, z).Transparent) {
                // 左
                r |= Left;
            }
            if (x >= ParamConst.ChunkSize || GetBlock(x, y + 1, z).Transparent) {
                // 右
                r |= Right;
            }
            if (y < 1 || GetBlock(x, y + 1, z).Transparent) {
                // 上
                r |= Up;
            }
            if (y >= ParamConst.ChunkSize || GetBlock(x, y - 1, z).Transparent) {
                // 下
                r |= Down;
            }
            if (z < 1 || GetBlock(x, y, z - 1).Transparent) {
                // 前
                r |= Front;
            }
            if (z >= ParamConst.ChunkSize || GetBlock(x, y, z + 1).Transparent) {
                // 后
                r |= Back;
            }
            block.RenderFlags = r;
            BlockData[x, y, z] = block;
        }
    }
}