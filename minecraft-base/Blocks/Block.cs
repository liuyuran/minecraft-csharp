using System.Collections.Generic;

namespace Base.Blocks {
    /// <summary>
    /// 地形基础构成，所有方块的基类
    /// </summary>
    public abstract class Block {
        public virtual string ID => ""; // 方块id
        public virtual string Texture => ""; // 贴图，如果是空，则为透明
        public IDictionary<string, string> Meta = new Dictionary<string, string>(); // 扩展数据
        public int RenderFlags = 0; // 以二进制形式存储六个面是否需要渲染
        public virtual bool Transparent => false; // 是否透明
        public virtual int Resistance => 100; // 行进阻力
    }
}