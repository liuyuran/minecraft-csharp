﻿using System;
using System.Numerics;
using Base.Components;
using Base.Const;
using Base.Interface;
using Base.Manager;
using Base.Utils;

namespace Base.Systems {
    /// <summary>
    /// 向客户端同步生物和地图状态
    /// </summary>
    public class StatusSyncSystem: ISystem {
        public void OnCreate() {
            //
        }

        public void OnUpdate() {
            foreach (var entity in EntityManager.QueryByComponents(typeof(Player), typeof(Position))) {
                var player = entity.GetComponent<Player>();
                if (player.LastSyncTime + ParamConst.SyncInterval > DateTimeOffset.UtcNow.ToUnixTimeSeconds()) continue;
                // 同步地图数据
                var position = entity.GetComponent<Position>();
                for (var x = -ParamConst.DisplayDistance; x <= ParamConst.DisplayDistance; x++) {
                    for (var y = -ParamConst.DisplayDistance; y <= ParamConst.DisplayDistance; y++) {
                        for (var z = -ParamConst.DisplayDistance; z <= ParamConst.DisplayDistance; z++) {
                            var chunk = ChunkManager.GetChunk(0, new Vector3(
                                position.X + x,
                                position.Y + y,
                                position.Z + z
                            ));
                            if (chunk == null) continue;
                            CommandTransferManager.UpdateChunkForUser(chunk, player.Uuid);
                        }
                    }
                }
                // 重置计时器
                player.LastSyncTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
        }
    }
}