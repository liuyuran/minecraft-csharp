using System.Numerics;
using Base.Blocks;
using Base.Const;

namespace Base.Utils {
    public sealed class Chunk {
        public IBlock[,,] BlockData = new IBlock[ParamConst.ChunkSize,ParamConst.ChunkSize,ParamConst.ChunkSize];
        public Vector3 Position;
        public bool IsEmpty;
    }
}