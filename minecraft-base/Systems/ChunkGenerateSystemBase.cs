using System;
using System.Collections.Generic;
using System.Numerics;
using Base.Components;
using Base.Const;
using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 区块生成系统
    /// </summary>
    public class ChunkGenerateSystemBase : SystemBase {
        private readonly Dictionary<Vector3, long> _activeChunks = new();

        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            // 将玩家周围的区块生成并激活
            foreach (var entity in EntityManager.QueryByComponents(typeof(Player), typeof(Position))) {
                var position = entity.GetComponent<Position>();
                for (var x = -ParamConst.DisplayDistance; x <= ParamConst.DisplayDistance; x++) {
                    for (var y = -ParamConst.DisplayDistance; y <= ParamConst.DisplayDistance; y++) {
                        for (var z = -ParamConst.DisplayDistance; z <= ParamConst.DisplayDistance; z++) {
                            var pos = new Vector3(
                                position.X + x,
                                position.Y + y,
                                position.Z + z
                            );
                            _activeChunks[pos] = now;
                            ChunkManager.Instance.GenerateChunk(0, pos);
                        }
                    }
                }
            }
            // 将长时间没激活的区块卸载
            var removeList = new List<Vector3>();
            foreach (var (pos, time) in _activeChunks) {
                if (time + ParamConst.ChunkUnloadDelay < now) {
                    removeList.Add(pos);
                }
            }
            foreach (var pos in removeList) {
                _activeChunks.Remove(pos);
                ChunkManager.Instance.UnloadChunk(0, pos);
            }
        }
    }
}