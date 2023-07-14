using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading;
using Base.Blocks;
using Base.Components;
using Base.Const;
using Base.Interface;
using Base.Items;
using Base.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Base.Manager {
    public class ItemConverter : JsonConverter {
        private readonly IDictionary<string, Type> _itemMapping;
        
        public ItemConverter(IDictionary<string, Type> typeItemMapping) {
            _itemMapping = typeItemMapping;
        }
        
        private static readonly JsonSerializerSettings SpecifiedSubclassConversion =
            new() { ContractResolver = new DefaultContractResolver() };

        public override bool CanConvert(Type objectType) {
            return objectType == typeof(Item);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer) {
            var jo = JObject.Load(reader);
            var itemId = (jo["ID"] ?? "").Value<string>() ?? "";
            if (!_itemMapping.ContainsKey(itemId)) throw new Exception($"方块{itemId}已经无法找到");
            var type = _itemMapping[itemId];
            var result = JsonConvert.DeserializeObject(jo.ToString(), type, SpecifiedSubclassConversion);
            return result ?? throw new Exception($"方块{itemId}反序列化失败");
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
    
    /// <summary>
    /// 存档管理器
    /// </summary>
    public class ArchiveManager {
        public static ArchiveManager Instance { get; } = new();
        private readonly IDictionary<string, Type> _itemDict = new Dictionary<string, Type>();
        
        private string _archiveName = "default";
        
        private ArchiveManager() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var assem = typeof(Item).Assembly;
            var baseType = typeof(Item);
            var types = assem.GetExportedTypes();
            foreach (var type in types) {
                if (type.IsAbstract || type.FullName == null) continue;
                if (!type.IsSubclassOf(baseType)) continue;
                if (assem.CreateInstance(
                        type.FullName, false,
                        BindingFlags.ExactBinding,
                        null, new object[] { }, null, null
                    ) is not Item instance) continue;
                _itemDict.Add(instance.ID, type);
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
            var filename = $"{_archiveName}/chunk/{worldId}/{IntToHex(pos.X)}_{IntToHex(pos.Y)}_{IntToHex(pos.Z)}.json";
            if (!File.Exists(filename)) {
                return null;
            }
            var jsonData = JObject.Parse(File.ReadAllText(filename));
            var chunk = new Chunk {
                BlockData = jsonData.SelectToken("BlockData")?.ToObject<Block[]>() ?? Array.Empty<Block>(),
                WorldId = (int)(jsonData.SelectToken("WorldId") ?? 0),
                Position = jsonData.SelectToken("Position")?.ToObject<Vector3>() ?? Vector3.Zero,
                Version = (int)(jsonData.SelectToken("Version") ?? 0),
                IsEmpty = (bool)(jsonData.SelectToken("IsEmpty") ?? false)
            };
            return chunk;
        }

        public void SaveChunk(int worldId, Vector3 pos, Chunk chunkData) {
            // 是的我知道用json会让存档变得很大，但是二进制存储的兼容性想做的很好难度颇高，日后等大神实现吧，我不想努力了
            var path = $"{_archiveName}/chunk/{worldId}/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var data = new JObject {
                ["BlockData"] = JToken.FromObject(chunkData.BlockData),
                ["WorldId"] = chunkData.WorldId,
                ["Position"] = JToken.FromObject(chunkData.Position),
                ["Version"] = chunkData.Version,
                ["IsEmpty"] = chunkData.IsEmpty
            };
            var filename = $"{path}/{IntToHex(pos.X)}_{IntToHex(pos.Y)}_{IntToHex(pos.Z)}.json";
            File.WriteAllText(filename, data.ToString());
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
            return Convert.ToString((int)value, 16).PadLeft(16, '0');
        }
    }
}