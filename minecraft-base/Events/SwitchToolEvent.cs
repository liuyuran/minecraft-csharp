using Base.Interface;

namespace Base.Events {
    public class SwitchToolEvent: GameEvent {
        public bool isLeft;
        public int InventorySlot;
    }
}