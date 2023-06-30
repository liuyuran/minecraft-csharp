using System;
using System.Numerics;
using Base.Components;
using Base.Const;
using Base.Interface;
using Base.Manager;
using Base.Messages;

namespace Base.Systems {
    /// <summary>
    /// 向客户端同步生物和地图状态
    /// </summary>
    public class StatusSyncSystem : SystemBase {
        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(Player), typeof(Transform))) {
                var player = entity.GetComponent<Player>();
                if (player.LastSyncTime + ParamConst.SyncInterval > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    continue;
                // 同步地图数据
                var position = entity.GetComponent<Transform>().Position + new Vector3();
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
                                Chunk = chunk
                            });
                        }
                    }
                }

                // 重置计时器
                player.LastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
    }
}