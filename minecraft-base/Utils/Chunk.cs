using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Base.Blocks;
using Base.Const;
using Base.Manager;

namespace Base.Utils {
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class Chunk {
        // 六个面的可见性掩码
        public const int Left = 0b1;
        public const int Right = 0b10;
        public const int Up = 0b100;
        public const int Down = 0b1000;
        public const int Front = 0b10000;
        public const int Back = 0b100000;

        public readonly Block[,,] BlockData =
            new Block[ParamConst.ChunkSize, ParamConst.ChunkSize, ParamConst.ChunkSize];

        public long Version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public int WorldId;
        public Vector3 Position;
        public bool IsEmpty;

        public Block GetBlock(int x, int y, int z) {
            return BlockData[x, y, z];
        }

        private Block? GetBlockCrossChunk(int x, int y, int z) {
            var target = this;
            while (x < 0) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(-1, 0, 0));
                if (target == null) {
                    return null;
                }

                x += ParamConst.ChunkSize;
            }

            while (x > ParamConst.ChunkSize - 1) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(1, 0, 0));
                if (target == null) {
                    return null;
                }

                x -= ParamConst.ChunkSize;
            }

            while (y < 0) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(0, -1, 0));
                if (target == null) {
                    return null;
                }

                y += ParamConst.ChunkSize;
            }

            while (y > ParamConst.ChunkSize - 1) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(0, 1, 0));
                if (target == null) {
                    return null;
                }

                y -= ParamConst.ChunkSize;
            }

            while (z < 0) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(0, 0, -1));
                if (target == null) {
                    return null;
                }

                z += ParamConst.ChunkSize;
            }

            while (z > ParamConst.ChunkSize - 1) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(0, 0, 1));
                if (target == null) {
                    return null;
                }

                z -= ParamConst.ChunkSize;
            }

            return target.GetBlock(x, y, z);
        }

        public void SetBlock(int x, int y, int z, Block block) {
            var r = 0;
            if (GetBlockCrossChunk(x - 1, y, z)?.Transparent ?? true) {
                // 左
                r |= Left;
            }

            if (GetBlockCrossChunk(x + 1, y, z)?.Transparent ?? true) {
                // 右
                r |= Right;
            }

            if (GetBlockCrossChunk(x, y - 1, z)?.Transparent ?? true) {
                // 下
                r |= Down;
            }

            if (GetBlockCrossChunk(x, y + 1, z)?.Transparent ?? true) {
                // 上
                r |= Up;
            }

            if (GetBlockCrossChunk(x, y, z - 1)?.Transparent ?? true) {
                // 前
                r |= Front;
            }

            if (GetBlockCrossChunk(x, y, z + 1)?.Transparent ?? true) {
                // 后
                r |= Back;
            }

            block.RenderFlags = r;
            BlockData[x, y, z] = block;
        }
    }
}