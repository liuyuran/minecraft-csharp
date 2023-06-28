using System.Collections.Generic;

namespace Base.Blocks {
    /// <summary>
    /// 地形基础构成，所有方块的基类
    /// </summary>
    public abstract class Block {
        public enum BlockType {
            Air, // 气体 
            Solid, // 固体
            Liquid // 液体
        }
        
        public virtual string ID => ""; // 方块id
        public virtual string Icon => ""; // 方块图标
        public virtual string Texture => ""; // 贴图，如果是空，则为透明
        public virtual string Name => ""; // 方块名称
        public virtual string Description => ""; // 方块描述
        public IDictionary<string, string> Meta = new Dictionary<string, string>(); // 扩展数据
        public int RenderFlags = 0; // 以二进制形式存储六个面是否需要渲染
        public int BreakProgress = 0; // 破坏进度，取值为0-MaxDurability，当到达最大值，方块被破坏，前提是MaxDurability不为-1
        public virtual int MaxDurability => -1; // 最大耐久度，-1代表无穷大，不可破坏
        public virtual int HardnessLevel => -1; // 采集等级，和Type共同决定可以用来采集的工具，-1代表无穷大，不可采集
        public virtual bool Transparent => false; // 是否透明
        public virtual int Resistance => 0; // 行进阻力，取值为0-100，越大减速效果越强，对于固体，越大在其上行走的减速效果越强，对于液体和气体，则同时影响流速和在其中行走的速度
        public virtual BlockType Type => BlockType.Solid; // 方块类型
    }
}