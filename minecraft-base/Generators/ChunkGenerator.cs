using System.Numerics;
using Base.Blocks;
using Base.Const;
using Base.Utils;

namespace Base.Generators {
    public static class ChunkGenerator {
        /// <summary>
        /// 生成地下地形
        /// </summary>
        /// <returns>生成好的区块</returns>
        public static Chunk GenerateUnderGroundChunk() {
            var chunk = new Chunk();
            for (var x = 0; x < ParamConst.ChunkSize; x++) {
                for (var y = 0; y < ParamConst.ChunkSize; y++) {
                    for (var z = 0; z < ParamConst.ChunkSize; z++) {
                        chunk.BlockData[x, y, z] = new Bedrock();
                    }
                }
            }

            chunk.IsEmpty = false;
            return chunk;
        }

        /// <summary>
        /// 创建普通地形，预期使用波函数坍塌算法生成地形
        /// 好吧，想多了，扩展结构可以用波函数塌缩，但是纯地形只能用柏林噪声了
        /// </summary>
        /// <param name="north">北方区块（z正方向）</param>
        /// <param name="south">南方区块（z负方向）</param>
        /// <param name="east">东方区块（x正方向）</param>
        /// <param name="west">西方区块（x负方向）</param>
        /// <param name="top">上方区块（y正方向）</param>
        /// <param name="bottom">下方区块（y负方向）</param>
        /// <returns>生成好的区块</returns>
        public static Chunk GenerateNormalChunk(Chunk? north, Chunk? south, Chunk? east, Chunk? west, Chunk? top, Chunk? bottom) {
            var chunk = new Chunk {
                Position = new Vector3(0, 0, 0),
                IsEmpty = true
            };
            for (var x = 0; x < ParamConst.ChunkSize; x++) {
                for (var z = 0; z < ParamConst.ChunkSize; z++) {
                    // TODO 这里的柏林噪声似乎返回了一个固定值，这是为啥呢？
                    var target = 1.0f;
                    for (var y = 0; y < ParamConst.ChunkSize; y++) {
                        if (y < target) chunk.BlockData[x, y, z] = new Bedrock();
                        else chunk.BlockData[x, y, z] = new Air();
                    }
                }
            }

            chunk.IsEmpty = false;
            return chunk;
        }

        /// <summary>
        /// 创建天空地形
        /// </summary>
        /// <param name="north">北方区块（z正方向）</param>
        /// <param name="south">南方区块（z负方向）</param>
        /// <param name="east">东方区块（x正方向）</param>
        /// <param name="west">西方区块（x负方向）</param>
        /// <param name="top">上方区块（y正方向）</param>
        /// <param name="bottom">下方区块（y负方向）</param>
        /// <returns>生成好的区块</returns>
        public static Chunk GenerateSkyChunk(Chunk? north, Chunk? south, Chunk? east, Chunk? west, Chunk? top, Chunk? bottom) {
            var chunk = new Chunk {
                Position = new Vector3(0, 0, 0),
                IsEmpty = true
            };
            for (var x = 0; x < ParamConst.ChunkSize; x++) {
                for (var z = 0; z < ParamConst.ChunkSize; z++) {
                    for (var y = 0; y < ParamConst.ChunkSize; y++) {
                        chunk.BlockData[x, y, z] = new Air();
                    }
                }
            }

            chunk.IsEmpty = true;
            return chunk;
        }
    }
}