using System.Collections.Generic;
using Base.Blocks;
using Newtonsoft.Json;
using ProtoBuf;

namespace Base.Items {
    public abstract class Item {
        public virtual string ID => ""; // 物品id
        [JsonIgnore]
        public virtual string Icon => ""; // 物品图标
        [JsonIgnore]
        public virtual string Name => ""; // 物品名称
        [JsonIgnore]
        public virtual string Description => ""; // 物品描述
        [JsonIgnore]
        public virtual int ItemType => (int)Enums.ItemType.None; // 物品类型
        [JsonIgnore]
        public virtual int MaxDurability => -1; // 最大耐久
        public int Durability = -1; // 当前耐久
        public IDictionary<string, string> Meta = new Dictionary<string, string>(); // 扩展数据

        /// <summary>
        /// 是否可以用于某种用途
        /// 出于节省点空间是点的考虑，使用位运算来表示物品类型
        /// </summary>
        /// <param name="type">工具类型</param>
        /// <returns>是否可用</returns>
        public bool CanDig(Block type) {
            return ((1 << (int)type.DigRequire) & ItemType) > 0;
        }
    }
}