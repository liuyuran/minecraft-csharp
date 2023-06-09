namespace Base.Blocks {
    /// <summary>
    /// 地形基础构成，所有方块的基类
    /// </summary>
    public interface IBlock {
        public string ID => "";
        public string Texture => "";
        public string Nbt => "";
        public bool IsAir => false;
        public bool IsWater => false;
    }
}