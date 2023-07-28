using Base.Interface;
using Base.Manager;

namespace Base.Systems {
    public class TickIncreaseSystem: SystemBase {
        public override void OnCreate() {
            //
        }

        public override void OnUpdate() {
            TimeManager.Instance.Update();
        }
    }
}