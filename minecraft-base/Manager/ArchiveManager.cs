using System;
using System.IO;
using System.Numerics;
using Base.Const;
using Base.Utils;

namespace Base.Manager {
    /// <summary>
    /// 存档管理器
    /// </summary>
    public class ArchiveManager {
        public static ArchiveManager Instance { get; } = new();
        private string _archiveName = "default";

        public void LoadArchive(string worldName) {
            _archiveName = ParamConst.ArchiveBasePath + "/" + worldName;
            if (!Directory.Exists(_archiveName)) {
                Console.WriteLine("存档目录不存在，开始新建目录结构");
                Directory.CreateDirectory(_archiveName);
                Directory.CreateDirectory($"{_archiveName}/chunk"); // 区块
                Directory.CreateDirectory($"{_archiveName}/creature"); // 生物
                Directory.CreateDirectory($"{_archiveName}/player"); // 玩家
                Directory.CreateDirectory($"{_archiveName}/setting"); // 设置
            }
            LoadSetting();
        }

        public void SaveArchive() {
            // TODO 目前看只需要保存地形和生物信息
        }

        private void LoadSetting() { }
        
        private void SaveSetting() { }

        public Chunk? LoadChunk(int worldId, Vector3 pos) {
            return null;
        }

        public void SaveChunk(int worldId, Vector3 pos, Chunk chunkData) { }

        public Entity? LoadPlayer(int worldId) {
            return null;
        }

        public void SavePlayer(int worldId, Entity playerData) { }
    }
}