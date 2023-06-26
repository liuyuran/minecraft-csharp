using Base.Components;
using Base.Components.Tags;
using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 死亡检测系统
    /// </summary>
    public class CheckDeadSystem: SystemBase {
        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            foreach (var entity in EntityManager.QueryByComponents(typeof(Health))) {
                var component = entity.GetComponent<Health>();
                if (component.health <= 0 && !entity.HasComponent<IsDead>()) {
                    entity.AddComponent<IsDead>();
                }
            }
        }
    }
}