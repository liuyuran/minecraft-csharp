using Base.Interface;

namespace Base.Events.ClientEvent {
    public class SwitchToolEvent: GameEvent {
        public bool isLeft;
        public int InventorySlot;
    }
}