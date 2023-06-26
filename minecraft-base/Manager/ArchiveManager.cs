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

        public void SaveChunk(int worldId, Vector3 pos, Chunk chunkData) {
            //
        }

        public void LoadArchive(string worldName) {
            _archiveName = ParamConst.ArchiveBasePath + "/" + worldName;
            if (!Directory.Exists(_archiveName)) {
                Console.WriteLine("存档目录不存在，开始新建目录结构");
                Directory.CreateDirectory(_archiveName);
            }
        }

        public void SaveArchive() {
            //
        }
    }
}