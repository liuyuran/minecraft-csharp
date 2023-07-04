using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Base.Blocks;
using Base.Const;
using Base.Manager;
using ProtoBuf;

namespace Base.Utils {
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [ProtoContract(SkipConstructor=true)]
    public sealed class Chunk {
        // 六个面的可见性掩码
        public const int Left = 0b1;
        public const int Right = 0b10;
        public const int Up = 0b100;
        public const int Down = 0b1000;
        public const int Front = 0b10000;
        public const int Back = 0b100000;

        [ProtoMember(1)]
        private readonly Block[] _blockData = new Block[ParamConst.ChunkSize * ParamConst.ChunkSize * ParamConst.ChunkSize];
        [ProtoMember(2)]
        public long Version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        [ProtoMember(3)]
        public int WorldId;
        [ProtoMember(4)]
        public Vector3 Position;
        [ProtoMember(5)]
        public bool IsEmpty;

        public Block GetBlock(int x, int y, int z) {
            return _blockData[x * ParamConst.ChunkSize * ParamConst.ChunkSize + y * ParamConst.ChunkSize + z];
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
            _blockData[x * ParamConst.ChunkSize * ParamConst.ChunkSize + y * ParamConst.ChunkSize + z] = block;
        }

        public Block? GetBlockCrossChunk(int x, int y, int z) {
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
        
        public void SetBlockCrossChunk(int x, int y, int z, Block block) {
            var target = this;
            while (x < 0) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(-1, 0, 0));
                if (target == null) {
                    return;
                }

                x += ParamConst.ChunkSize;
            }

            while (x > ParamConst.ChunkSize - 1) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(1, 0, 0));
                if (target == null) {
                    return;
                }

                x -= ParamConst.ChunkSize;
            }

            while (y < 0) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(0, -1, 0));
                if (target == null) {
                    return;
                }

                y += ParamConst.ChunkSize;
            }

            while (y > ParamConst.ChunkSize - 1) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(0, 1, 0));
                if (target == null) {
                    return;
                }

                y -= ParamConst.ChunkSize;
            }

            while (z < 0) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(0, 0, -1));
                if (target == null) {
                    return;
                }

                z += ParamConst.ChunkSize;
            }

            while (z > ParamConst.ChunkSize - 1) {
                target = ChunkManager.Instance.GetChunk(WorldId, Position + new Vector3(0, 0, 1));
                if (target == null) {
                    return;
                }

                z -= ParamConst.ChunkSize;
            }

            target.SetBlock(x, y, z, block);
        }
    }
}