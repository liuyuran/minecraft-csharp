using Base.Events.InnerBus;

namespace Base.Utils {
    public class EventBus {
        public static EventBus Instance { get; } = new();
        public event BlockBreakEventHandler? BlockBreakEvent;
        public event BlockHitEventHandler? BlockHitEvent;
        public event ItemUsedEventHandler? ItemUsedEvent;

        public void OnBlockBreakEvent(BlockBreakEvent evt) {
            BlockBreakEvent?.Invoke(evt);
        }
        
        public void OnBlockHitEvent(BlockHitEvent evt) {
            BlockHitEvent?.Invoke(evt);
        }
        
        public void OnItemUsedEvent(ItemUsedEvent evt) {
            ItemUsedEvent?.Invoke(evt);
        }
    }
}