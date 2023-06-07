namespace Base.Blocks {
    /// <summary>
    /// 地形基础构成，所有方块的基类
    /// </summary>
    public interface IBlock {
        public virtual string ID => "";
        public virtual string Texture => "";
        public virtual string Nbt => "";
    }
}