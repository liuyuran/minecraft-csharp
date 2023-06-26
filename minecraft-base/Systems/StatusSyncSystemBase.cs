using System;
using System.Numerics;
using Base.Components;
using Base.Const;
using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 向客户端同步生物和地图状态
    /// </summary>
    public class StatusSyncSystemBase : SystemBase {
        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            foreach (var entity in EntityManager.QueryByComponents(typeof(Player), typeof(Transform))) {
                var player = entity.GetComponent<Player>();
                if (player.LastSyncTime + ParamConst.SyncInterval > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    continue;
                // 同步地图数据
                var position = entity.GetComponent<Transform>().Position;
                for (var x = -ParamConst.DisplayDistance; x <= ParamConst.DisplayDistance; x++) {
                    for (var y = -ParamConst.DisplayDistance; y <= ParamConst.DisplayDistance; y++) {
                        for (var z = -ParamConst.DisplayDistance; z <= ParamConst.DisplayDistance; z++) {
                            var pos = position + new Vector3(x, y, z);
                            var chunk = ChunkManager.Instance.GetChunk(0, pos);
                            if (chunk == null) continue;
                            CommandTransferManager.NetworkAdapter?.UpdateChunkForUser(chunk, player.Uuid);
                        }
                    }
                }

                // 重置计时器
                player.LastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
    }
}