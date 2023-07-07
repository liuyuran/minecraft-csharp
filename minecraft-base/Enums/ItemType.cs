namespace Base.Enums {
    /// <summary>
    /// 需要按位运算赋值，不能直接等于
    /// </summary>
    public enum ItemType {
        None = -1, // 无交互
        Hand = 0, // 手，基础挖掘工具
        Block = 1, // 可放置方块
        Axe = 2, // 斧头
        Pickaxe = 3, // 镐子
        Shovel = 4, // 铲子
    }
}