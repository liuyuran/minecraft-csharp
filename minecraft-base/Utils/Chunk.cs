using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Base.Blocks;
using Base.Const;
using Base.Manager;
using ProtoBuf;

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

        public Block[] BlockData = new Block[ParamConst.ChunkSize * ParamConst.ChunkSize * ParamConst.ChunkSize];
        public long Version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public int WorldId;
        public Vector3 Position;
        public bool IsEmpty;

        public Block GetBlock(int x, int y, int z) {
            return BlockData[x * ParamConst.ChunkSize * ParamConst.ChunkSize + y * ParamConst.ChunkSize + z];
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

            if (!block.Transparent) {
                // 如果方块本身不是透明的，则更新六边可见性
                var left = GetBlockCrossChunk(x - 1, y, z);
                if (left is { Transparent: false }) {
                    left.RenderFlags &= ~Right;
                }
                var up = GetBlockCrossChunk(x, y + 1, z);
                if (up is { Transparent: false }l) {
                    up.RenderFlags &= ~Down;
                }
                var right = GetBlockCrossChunk(x + 1, y, z);
                if (right is { Transparent: false }) {
                    right.RenderFlags &= ~Left;
                }
                var down = GetBlockCrossChunk(x, y - 1, z);
                if (down is { Transparent: false }) {
                    down.RenderFlags &= ~Up;
                }
                var back = GetBlockCrossChunk(x, y, z + 1);
                if (back is { Transparent: false }) {
                    back.RenderFlags &= ~Front;
                }
                var front = GetBlockCrossChunk(x, y, z - 1);
                if (front is { Transparent: false }) {
                    front.RenderFlags &= ~Back;
                }
            }
            block.RenderFlags = r;
            BlockData[x * ParamConst.ChunkSize * ParamConst.ChunkSize + y * ParamConst.ChunkSize + z] = block;
        }

        public Block? GetBlockCrossChunk(int x, int y, int z) {
            var target = this;
            var blockPos = new Vector3(x, y, z);
            var chunkPos = Position + new Vector3();
            NormallyBlockPos(ref blockPos, ref chunkPos);
            if (chunkPos == Position) return target.GetBlock((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);
            target = ChunkManager.Instance.GetChunk(WorldId, chunkPos);
            return target?.GetBlock((int) blockPos.X, (int) blockPos.Y, (int) blockPos.Z);
        }

        public static void NormallyBlockPos(ref Vector3 blockPos, ref Vector3 chunkPos) {
            var x = (int) blockPos.X;
            var y = (int) blockPos.Y;
            var z = (int) blockPos.Z;
            while (x < 0) {
                x += ParamConst.ChunkSize;
                chunkPos.X -= 1;
            }

            while (x > ParamConst.ChunkSize - 1) {
                x -= ParamConst.ChunkSize;
                chunkPos.X += 1;
            }

            while (y < 0) {
                y += ParamConst.ChunkSize;
                chunkPos.Y -= 1;
            }

            while (y > ParamConst.ChunkSize - 1) {
                y -= ParamConst.ChunkSize;
                chunkPos.Y += 1;
            }

            while (z < 0) {
                z += ParamConst.ChunkSize;
                chunkPos.Z -= 1;
            }

            while (z > ParamConst.ChunkSize - 1) {
                z -= ParamConst.ChunkSize;
                chunkPos.Z += 1;
            }
            blockPos.X = x;
            blockPos.Y = y;
            blockPos.Z = z;
        }

        public void SetBlockCrossChunk(Vector3 blockPos, Block block) {
            var target = this;
            var chunkPos = Position + new Vector3();
            NormallyBlockPos(ref blockPos, ref chunkPos);
            if (chunkPos != Position) {
                target = ChunkManager.Instance.GetChunk(WorldId, chunkPos);
                if (target == null) {
                    return;
                }
            }

            target.SetBlock((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z, block);
        }
    }
}