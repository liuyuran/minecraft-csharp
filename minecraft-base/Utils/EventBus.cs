using Base.Blocks;

namespace Base.Utils {
    public class EventBus {
        public delegate void BlockUpdateEventHandler(Block e);
        public static event BlockUpdateEventHandler BlockUpdateEvent;
        public static void OnBlockUpdateEvent(Block e) {
            BlockUpdateEvent?.Invoke(e);
        }
    }
}