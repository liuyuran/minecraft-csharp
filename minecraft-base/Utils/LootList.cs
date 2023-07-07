using System.Collections.Generic;
using Base.Items;

namespace Base.Utils {
    public struct LootList {
        public Item Item;
        public int DropCount;
        public float Weight;
        public IDictionary<string, string> Meta;
    }
}