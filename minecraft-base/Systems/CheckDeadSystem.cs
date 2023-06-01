using Base.Components;
using Base.Components.Tags;
using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    /// <summary>
    /// 死亡检测系统
    /// </summary>
    public class CheckDeadSystem: ISystem {
        public void OnCreate() {
            //
        }

        public void OnUpdate() {
            foreach (var entity in EntityManager.QueryByComponents(typeof(Health))) {
                var component = entity.GetComponent<Health>();
                if (component.health <= 0 && !entity.HasComponent<IsDead>()) {
                    entity.AddComponent<IsDead>();
                }
            }
        }
    }
}