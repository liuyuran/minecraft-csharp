using System;
using System.Collections.Generic;
using System.Numerics;
using Base.Components;
using Base.Const;
using Base.Events;
using Base.Events.ServerEvent;
using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 向客户端同步生物和地图状态
    /// </summary>
    public class StatusSyncSystem : SystemBase {
        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            // 索引一下物品位置和区块的对应关系
            var items = new Dictionary<Vector3, Dictionary<Vector3, DroppedItem>>();
            foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(DroppedItem), typeof(Transform))) {
                var item = entity.GetComponent<DroppedItem>();
                var position = entity.GetComponent<Transform>().Position;
                position.X = (float)Math.Floor(position.X / ParamConst.ChunkSize);
                position.Y = (float)Math.Floor(position.Y / ParamConst.ChunkSize);
                position.Z = (float)Math.Floor(position.Z / ParamConst.ChunkSize);
                if (!items.ContainsKey(position)) {
                    items.Add(position, new Dictionary<Vector3, DroppedItem>());
                }

                items[position].Add(position, item);
            }

            foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(Player), typeof(Transform))) {
                var player = entity.GetComponent<Player>();
                if (player.LastSyncTime + ParamConst.SyncInterval > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    continue;
                var position = entity.GetComponent<Transform>().Position + new Vector3();
                // 同步地图数据
                UpdateChunkData(player, position, items);
                // 重置计时器
                player.LastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }

        /// <summary>
        /// 更新玩家附近带有物品信息的区块数据
        /// </summary>
        /// <param name="player">玩家信息</param>
        /// <param name="position">玩家位置</param>
        /// <param name="items">物品数据缓存</param>
        private static void UpdateChunkData(Player player, Vector3 position,
            IReadOnlyDictionary<Vector3, Dictionary<Vector3, DroppedItem>> items) {
            position.X = (float)Math.Round(position.X / ParamConst.ChunkSize);
            position.Y = (float)Math.Round(position.Y / ParamConst.ChunkSize);
            position.Z = (float)Math.Round(position.Z / ParamConst.ChunkSize);
            for (var x = -ParamConst.DisplayDistance; x <= ParamConst.DisplayDistance; x++) {
                for (var y = -ParamConst.DisplayDistance; y <= ParamConst.DisplayDistance; y++) {
                    for (var z = -ParamConst.DisplayDistance; z <= ParamConst.DisplayDistance; z++) {
                        var pos = position + new Vector3(x, y, z);
                        var chunk = ChunkManager.Instance.GetChunk(0, pos);
                        if (chunk == null) continue;
                        CommandTransferManager.NetworkAdapter?.SendToClient(player.Uuid, new ChunkUpdateEvent {
                            Chunk = chunk,
                            Items = items.TryGetValue(pos, out var value) ?
                                value :
                                new Dictionary<Vector3, DroppedItem>()
                        });
                    }
                }
            }
        }
    }
}