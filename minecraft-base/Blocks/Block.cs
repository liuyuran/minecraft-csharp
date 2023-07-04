using Base.Enums;
using Base.Items;
using ProtoBuf;

namespace Base.Blocks {
    /// <summary>
    /// 地形基础构成，所有方块的基类
    /// </summary>
    [ProtoContract]
    public abstract class Block: Item {
        public virtual string Texture => ""; // 贴图，如果是空，则为透明
        [ProtoMember(3)]
        public int RenderFlags = 0; // 以二进制形式存储六个面是否需要渲染
        public virtual int HardnessLevel => -1; // 采集等级，和Type共同决定可以用来采集的工具，-1代表无穷大，不可采集
        public virtual ItemType DigRequire => Enums.ItemType.None; // 可以用来采集的工具类型
        public virtual bool Transparent => false; // 是否透明
        public virtual int Resistance => 0; // 行进阻力，取值为0-100，越大减速效果越强，对于固体，越大在其上行走的减速效果越强，对于液体和气体，则同时影响流速和在其中行走的速度
        public virtual BlockType Type => BlockType.Solid; // 方块类型
    }
}