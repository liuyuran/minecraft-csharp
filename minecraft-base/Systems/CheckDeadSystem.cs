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
            foreach (var entity in EntityManager.Instance.QueryByComponents(typeof(HealthData))) {
                var component = entity.GetComponent<HealthData>();
                if (component.Health <= 0 && !entity.HasComponent<IsDead>()) {
                    entity.AddComponent<IsDead>();
                }
            }
        }
    }
}