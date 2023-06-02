namespace Base.Blocks {
    /// <summary>
    /// 地形基础构成，所有方块的基类
    /// </summary>
    public interface IBlock {
        public virtual string id => "";
        public virtual string nbt => "";
    }
}