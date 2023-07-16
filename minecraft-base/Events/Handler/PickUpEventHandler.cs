using System;
using Base.Components;
using Base.Events.ClientEvent;
using Base.Interface;
using Base.Items;
using Base.Manager;
using Base.Utils;

namespace Base.Events.Handler {
    public class PickUpEventHandler : IGameEventHandler<PickUpEvent> {
        private static Item ConvertIdToItem(string id) {
            ArchiveManager.Instance.ItemDict.TryGetValue(id, out var type);
            if (type != null) {
                return (Item) Activator.CreateInstance(type);
            }

            throw new Exception("未知物品");
        } 
        
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

            if (target == null || targetData == null) return;
            data.AddItem(ConvertIdToItem(targetData.ItemID));
            EntityManager.Instance.Destroy(target);
        }
    }
}