using System;
using Base.Blocks;
using Base.Const;
using Base.Utils;

namespace Base.Generators {
    public static class ChunkGenerator {
        /// <summary>
        /// 生成地下地形
        /// </summary>
        /// <param name="worldId">世界id</param>
        /// <param name="position">区块坐标</param>
        /// <returns>生成好的区块</returns>
        public static Chunk GenerateUnderGroundChunk(int worldId, Vector3 position) {
            var chunk = new Chunk {
                WorldId = worldId,
                Version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Position = position,
                IsEmpty = true
            };
            for (var x = 0; x < ParamConst.ChunkSize; x++) {
                for (var y = 0; y < ParamConst.ChunkSize; y++) {
                    for (var z = 0; z < ParamConst.ChunkSize; z++) {
                        chunk.SetBlock(x, y, z, new Dirt());
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
        /// <param name="worldId">世界id</param>
        /// <param name="position">区块坐标</param>
        /// <returns>生成好的区块</returns>
        public static Chunk GenerateNormalChunk(int worldId, Vector3 position) {
            var chunk = new Chunk {
                WorldId = worldId,
                Version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Position = position,
                IsEmpty = true
            };
            Perlin.Reseed();
            for (var x = 0; x < ParamConst.ChunkSize; x++) {
                for (var z = 0; z < ParamConst.ChunkSize; z++) {
                    var noise = Perlin.Noise((float)x / ParamConst.ChunkSize, (float)z / ParamConst.ChunkSize);
                    noise += 5;
                    noise /= 6;
                    var target = Math.Floor(noise * ParamConst.ChunkSize);
                    for (var y = 0; y < ParamConst.ChunkSize; y++) {
                        if (y < target) 
                            chunk.SetBlock(x, y, z, new Dirt());
                        else 
                            chunk.SetBlock(x, y, z, new Air());
                    }
                }
            }
            
            chunk.IsEmpty = false;
            return chunk;
        }

        /// <summary>
        /// 创建天空地形
        /// </summary>
        /// <param name="worldId">世界id</param>
        /// <param name="position">区块坐标</param>
        /// <returns>生成好的区块</returns>
        public static Chunk GenerateSkyChunk(int worldId, Vector3 position) {
            var chunk = new Chunk {
                WorldId = worldId,
                Version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Position = position,
                IsEmpty = true
            };
            for (var x = 0; x < ParamConst.ChunkSize; x++) {
                for (var z = 0; z < ParamConst.ChunkSize; z++) {
                    for (var y = 0; y < ParamConst.ChunkSize; y++) {
                        chunk.SetBlock(x, y, z, new Air());
                    }
                }
            }

            chunk.IsEmpty = true;
            return chunk;
        }
    }
}