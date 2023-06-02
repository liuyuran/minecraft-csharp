using System.Collections.Generic;
using System.Numerics;
using Base.Generators;
using Base.Utils;

namespace Base.Manager {
    /// <summary>
    /// 区块管理器，提供加载/卸载地图区域的功能
    /// </summary>
    public static class ChunkManager {
        private static readonly Dictionary<int, Dictionary<Vector3, Chunk>> MChunkData = new();

        public static Chunk? GetChunk(int worldId, Vector3 position) {
            var chunks = MChunkData[worldId];
            return chunks.TryGetValue(position, out var chunk) ? chunk : null;
        }
        
        /// <summary>
        /// 生成指定位置的Chunk
        /// </summary>
        /// <param name="worldId">世界ID</param>
        /// <param name="position">区块的世界坐标</param>
        public static void GenerateChunk(int worldId, Vector3 position) {
            if (!MChunkData.ContainsKey(worldId)) MChunkData.Add(worldId, new Dictionary<Vector3, Chunk>());
            if (MChunkData[worldId].ContainsKey(position)) return;
            switch (position.Y) {
                case 0:
                    MChunkData[worldId].Add(position, ChunkGenerator.GenerateNormalChunk(
                        GetChunk(worldId, position + new Vector3(0, 0, 1)),
                        GetChunk(worldId, position + new Vector3(0, 0, -1)),
                        GetChunk(worldId, position + new Vector3(1, 0, 0)),
                        GetChunk(worldId, position + new Vector3(-1, 0, 0)),
                        GetChunk(worldId, position + new Vector3(0, 1, 0)),
                        GetChunk(worldId, position + new Vector3(0, -1, 0))
                    ));
                    break;
                case > 0:
                    MChunkData[worldId].Add(position, ChunkGenerator.GenerateSkyChunk(
                        GetChunk(worldId, position + new Vector3(0, 0, 1)),
                        GetChunk(worldId, position + new Vector3(0, 0, -1)),
                        GetChunk(worldId, position + new Vector3(1, 0, 0)),
                        GetChunk(worldId, position + new Vector3(-1, 0, 0)),
                        GetChunk(worldId, position + new Vector3(0, 1, 0)),
                        GetChunk(worldId, position + new Vector3(0, -1, 0))
                    ));
                    break;
                default:
                    MChunkData[worldId].Add(position, ChunkGenerator.GenerateUnderGroundChunk());
                    break;
            }
            MChunkData[worldId][position].Position = position;
        }
    }
}