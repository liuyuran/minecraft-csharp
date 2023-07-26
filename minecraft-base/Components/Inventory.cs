using System;
using Base.Interface;
using Base.Items;

namespace Base.Components {
    /// <summary>
    /// 标记可以存储物品，同时存储物品栏信息
    /// </summary>
    public class Inventory: IComponentData {
        public int Size = 0;
        public Item?[] Items = Array.Empty<Item>();
        
        public void AddItem(Item item) {
            for (var i = 0; i < Size; i++) {
                if (Items[i] != null) continue;
                Items[i] = item;
                return;
            }
        }
    }
}