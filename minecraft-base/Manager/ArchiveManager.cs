using System;
using System.IO;
using System.Numerics;
using System.Threading;
using Base.Blocks;
using Base.Components;
using Base.Const;
using Base.Items;
using Base.Utils;
using ProtoBuf;
using ProtoBuf.Meta;
using Newtonsoft.Json.Linq;

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
            PlayerManager.Instance.GetAllPlayer().ForEach(SavePlayer);
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

        public Entity LoadPlayer(string uuid, string nickname) {
            var filename = $"{_archiveName}/player/{uuid}.json";
            var player = EntityManager.Instance.Instantiate();
            player.AddComponent(new Player {
                Uuid = uuid,
                LastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                LastControlTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                NickName = nickname
            });

            var defaultData = new JObject();
            if (File.Exists(filename)) {
                defaultData = JObject.Parse(File.ReadAllText(filename));
            }

            player.AddComponent(new Transform {
                Position = new Vector3(
                    (float)(defaultData.SelectToken("position.x") ?? 0),
                    (float)(defaultData.SelectToken("position.y") ?? 0),
                    (float)(defaultData.SelectToken("position.z") ?? 0)
                ),
                Forward = new Vector3(
                    (float)(defaultData.SelectToken("forward.x") ?? 0),
                    (float)(defaultData.SelectToken("forward.y") ?? 0),
                    (float)(defaultData.SelectToken("forward.z") ?? 0)
                )
            });
            player.AddComponent(new World {
                WorldId = (int)(defaultData.SelectToken("worldId") ?? 0)
            });
            player.AddComponent<HealthData>();
            player.AddComponent<Equipment>();
            player.AddComponent(new Inventory {
                Size = 32,
                Items = new Item[32]
            });
            player.AddComponent<ToolInHand>();
            return player;
        }

        private void SavePlayer(Entity playerData) {
            var playerId = playerData.GetComponent<Player>().Uuid;
            var filename = $"{_archiveName}/player/{playerId}.json";
            var res = new JObject {
                ["uuid"] = playerId,
                ["nickname"] = playerData.GetComponent<Player>().NickName,
                // 世界、位置和朝向
                ["worldId"] = playerData.GetComponent<World>().WorldId,
                ["position"] = new JObject {
                    ["x"] = playerData.GetComponent<Transform>().Position.X,
                    ["y"] = playerData.GetComponent<Transform>().Position.Y,
                    ["z"] = playerData.GetComponent<Transform>().Position.Z
                },
                ["forward"] = new JObject {
                    ["x"] = playerData.GetComponent<Transform>().Forward.X,
                    ["y"] = playerData.GetComponent<Transform>().Forward.Y,
                    ["z"] = playerData.GetComponent<Transform>().Forward.Z
                }
            };
            for (var i = 1; i <= 3; i++) {
                try {
                    File.WriteAllText(filename, res.ToString());
                    break;
                } catch (IOException) {
                    Thread.Sleep(100);
                }
            }
        }

        private static string IntToHex(float value) {
            return Convert.ToString((int)value, 16);
        }
    }
}