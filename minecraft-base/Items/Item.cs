using System.Collections.Generic;
using Base.Blocks;
using ProtoBuf;

namespace Base.Items {
    [ProtoContract]
    public abstract class Item {
        public virtual string ID => ""; // 物品id
        public virtual string Icon => ""; // 物品图标
        public virtual string Name => ""; // 物品名称
        public virtual string Description => ""; // 物品描述
        protected virtual int ItemType => 0; // 物品类型
        public virtual int MaxDurability => -1; // 最大耐久
        [ProtoMember(1)]
        public int Durability = -1; // 当前耐久
        [ProtoMember(2)]
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