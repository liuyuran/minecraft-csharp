using System.Collections.Generic;
using Base.Interface;

namespace Base.Components {
    public class DroppedItem: IComponentData {
        public string ItemID = "";
        public int Count = 1;
        public IDictionary<string, string> Meta = new Dictionary<string, string>(); // 扩展数据
    }
}