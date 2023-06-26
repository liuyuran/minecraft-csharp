using System.Collections.Generic;
using System.Numerics;
using Base.Generators;
using Base.Utils;

namespace Base.Manager {
    /// <summary>
    /// 区块管理器，提供加载/卸载地图区域的功能
    /// </summary>
    public class ChunkManager {
        public static ChunkManager Instance { get; } = new();
        private readonly Dictionary<int, Dictionary<Vector3, Chunk>> _mChunkData = new();

        /// <summary>
        /// 获取区块信息
        /// </summary>
        /// <param name="worldId">世界ID</param>
        /// <param name="position">区块世界坐标</param>
        /// <returns></returns>
        public Chunk? GetChunk(int worldId, Vector3 position) {
            var chunks = _mChunkData[worldId];
            return chunks.TryGetValue(position, out var chunk) ? chunk : null;
        }

        /// <summary>
        /// 生成指定位置的Chunk
        /// </summary>
        /// <param name="worldId">世界ID</param>
        /// <param name="position">区块的世界坐标</param>
        private void GenerateChunk(int worldId, Vector3 position) {
            if (!_mChunkData.ContainsKey(worldId)) _mChunkData.Add(worldId, new Dictionary<Vector3, Chunk>());
            if (_mChunkData[worldId].ContainsKey(position)) return;
            switch (position.Y) {
                case 0:
                    _mChunkData[worldId].Add(position, ChunkGenerator.GenerateNormalChunk(worldId, position));
                    break;
                case > 0:
                    _mChunkData[worldId].Add(position, ChunkGenerator.GenerateSkyChunk(worldId, position));
                    break;
                default:
                    _mChunkData[worldId].Add(position, ChunkGenerator.GenerateUnderGroundChunk(worldId, position));
                    break;
            }
            _mChunkData[worldId][position].Position = position;
        }
        
        /// <summary>
        /// 卸载指定位置的区块
        /// </summary>
        /// <param name="worldId">世界ID</param>
        /// <param name="position">区块世界坐标</param>
        public void UnloadChunk(int worldId, Vector3 position) {
            if (!_mChunkData.ContainsKey(worldId)) return;
            if (!_mChunkData[worldId].ContainsKey(position)) return;
            ArchiveManager.Instance.SaveChunk(worldId, position, _mChunkData[worldId][position]);
            _mChunkData[worldId].Remove(position);
        }
        
        /// <summary>
        /// 尝试从硬盘读取区块，如果不存在则生成区块
        /// </summary>
        /// <param name="worldId">世界ID</param>
        /// <param name="position">区块世界坐标</param>
        public void TryLoadChunkFromDisk(int worldId, Vector3 position) {
            if (!_mChunkData.ContainsKey(worldId)) _mChunkData.Add(worldId, new Dictionary<Vector3, Chunk>());
            if (_mChunkData[worldId].ContainsKey(position)) return;
            var chunk = ArchiveManager.Instance.LoadChunk(worldId, position);
            if (chunk == null) {
                GenerateChunk(worldId, position);
                ArchiveManager.Instance.SaveChunk(worldId, position, _mChunkData[worldId][position]);
            } else {
                _mChunkData[worldId].Add(position, chunk);                
            }
        }
    }
}