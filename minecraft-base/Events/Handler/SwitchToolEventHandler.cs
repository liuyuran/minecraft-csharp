using Base.Components;
using Base.Interface;
using Base.Items;
using Base.Manager;

namespace Base.Events.Handler {
    public class SwitchToolEventHandler: IGameEventHandler<SwitchToolEvent> {
        public void Run(SwitchToolEvent e) {
            var player = PlayerManager.Instance.GetPlayer(e.UserID);
            if (player == null) return;
            var inventory = player.GetComponent<Inventory>();
            if (e.InventorySlot > inventory.Items.Length - 1) return;
            var item = inventory.Items[e.InventorySlot];
            if (item == null) return;
            var hand = player.GetComponent<ToolInHand>();
            if (e.isLeft) {
                hand.LeftHand = item;
            } else {
                hand.RightHand = item;
            }
            inventory.Items[e.InventorySlot] = null;
        }
    }
}