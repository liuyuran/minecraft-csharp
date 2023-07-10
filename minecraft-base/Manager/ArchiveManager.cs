using System;
using System.IO;
using System.Numerics;
using Base.Blocks;
using Base.Const;
using Base.Utils;
using ProtoBuf;
using ProtoBuf.Meta;
using Microsoft.Data.Sqlite;

namespace Base.Manager {
    /// <summary>
    /// 存档管理器
    /// </summary>
    public class ArchiveManager {
        public static ArchiveManager Instance { get; } = new();
        private string _archiveName = "default";

        private ArchiveManager() {
            RuntimeTypeModel.Default
                .Add(typeof(Vector3), false)
                .Add("X", "Y", "Z");
            var subTypeDefine = RuntimeTypeModel.Default
                .Add(typeof(Block));
            var assem = typeof(Block).Assembly;
            var baseType = typeof(Block);
            var types = assem.GetExportedTypes();
            for (var index = 0; index < types.Length; index++) {
                var type = types[index];
                if (type.IsAbstract || type.FullName == null) continue;
                if (!type.IsSubclassOf(baseType)) continue;
                subTypeDefine.AddSubType(index + 1, type);
            }
        }

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
            var path = $"{_archiveName}/chunk/{worldId}/";
            if (!Directory.Exists(path)) return null;
            var filename = $"{IntToHex(pos.X)}_{IntToHex(pos.Y)}_{IntToHex(pos.Z)}.bin";
            if (!File.Exists($"{path}/{filename}")) return null;
            using var file = File.OpenRead($"{path}/{filename}");
            var chunk = Serializer.Deserialize<Chunk>(file);
            return chunk;
        }

        public void SaveChunk(int worldId, Vector3 pos, Chunk chunkData) {
            var path = $"{_archiveName}/chunk/{worldId}/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            using var file = File.Create($"{path}/{IntToHex(pos.X)}_{IntToHex(pos.Y)}_{IntToHex(pos.Z)}.bin");
            Serializer.Serialize(file, chunkData);
        }

        public Entity? LoadPlayer(int worldId) {
            return null;
        }

        public void SavePlayer(Entity playerData) {
            var connectionString = $"Data Source={_archiveName}/player/player.db";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            // Create the table
            using (var command = connection.CreateCommand()) {
                command.CommandText = "CREATE TABLE IF NOT EXISTS Player (Id INTEGER PRIMARY KEY, Name TEXT)";
                command.ExecuteNonQuery();
            }
            using (var command = connection.CreateCommand()) {
                command.CommandText = "update Player set Name = 'test' where Id = 1";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        private static string IntToHex(float value) {
            return Convert.ToString((int) value, 16);
        }
    }
}