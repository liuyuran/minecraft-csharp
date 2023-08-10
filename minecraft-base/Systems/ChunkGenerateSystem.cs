using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Base.Components;
using Base.Const;
using Base.Interface;
using Base.Manager;
using Base.Utils;

namespace Base.Systems {
    /// <summary>
    /// 区块生成系统
    /// </summary>
    public class ChunkGenerateSystem : SystemBase {
        private readonly ConcurrentDictionary<Vector3, long> _activeChunks = new();

        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var generateRange = ParamConst.DisplayDistance + 3;
            // 将玩家周围的区块生成并激活
            foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(Player), typeof(Transform))) {
                var position = new Vector3(entity.GetComponent<Transform>().Position);
                position.X = (float)Math.Round(position.X / ParamConst.ChunkSize);
                position.Y = (float)Math.Round(position.Y / ParamConst.ChunkSize);
                position.Z = (float)Math.Round(position.Z / ParamConst.ChunkSize);
                for (var x = -generateRange; x <= generateRange; x++) {
                    for (var y = -generateRange; y <= generateRange; y++) {
                        for (var z = -generateRange; z <= generateRange; z++) {
                            var pos = position + new Vector3(x, y, z);
                            _activeChunks[pos] = now;
                            ChunkManager.Instance.TryLoadChunkFromDisk(0, pos);
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
                _activeChunks.Remove(pos, out _);
                ChunkManager.Instance.UnloadChunk(0, pos);
            }
        }
    }
}