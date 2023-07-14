using Base.Components;
using Base.Events.ClientEvent;
using Base.Interface;
using Base.Manager;
using Base.Utils;

namespace Base.Events.Handler {
    public class PickUpEventHandler : IGameEventHandler<PickUpEvent> {
        public void Run(PickUpEvent e) {
            var player = PlayerManager.Instance.GetPlayer(e.UserID);
            if (player == null) return;
            var data = player.GetComponent<Inventory>();
            var allItems = EntityManager.Instance.QueryByComponents(typeof(DroppedItem), typeof(Transform));
            Entity? target = null;
            DroppedItem? targetData = null;
            foreach (var item in allItems) {
                var itemData = item.GetComponent<DroppedItem>();
                if (itemData.ItemID != e.ItemId) continue;
                target = item;
                targetData = itemData;
                break;
            }

            if (target == null) return;
            data.AddItem(targetData.ItemID);
            EntityManager.Instance.Destroy(target);
        }
    }
}