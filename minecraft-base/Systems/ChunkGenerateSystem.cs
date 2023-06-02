using System.Numerics;
using Base.Components;
using Base.Const;
using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 区块生成系统
    /// </summary>
    public class ChunkGenerateSystem : ISystem {
        public void OnCreate() {
            //
        }

        public void OnUpdate() {
            foreach (var entity in EntityManager.QueryByComponents(typeof(Player))) {
                var position = entity.GetComponent<Position>();
                for (var x = -ParamConst.DisplayDistance; x <= ParamConst.DisplayDistance; x++) {
                    for (var y = -ParamConst.DisplayDistance; y <= ParamConst.DisplayDistance; y++) {
                        for (var z = -ParamConst.DisplayDistance; z <= ParamConst.DisplayDistance; z++) {
                            ChunkManager.GenerateChunk(0, new Vector3(
                                position.X + x,
                                position.Y + y,
                                position.Z + z
                            ));
                        }
                    }
                }
            }
        }
    }
}