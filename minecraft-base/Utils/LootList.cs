using System.Collections.Generic;
using Base.Items;

namespace Base.Utils {
    public struct LootList {
        public string Item;
        public int DropCount;
        public float Weight;
        public IDictionary<string, string> Meta;
    }
}