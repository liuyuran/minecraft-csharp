namespace Base.Items {
    public abstract class Item {
        public virtual string ID => ""; // 物品id
        public virtual string Icon => ""; // 物品图标
        public virtual string Name => ""; // 物品名称
        public virtual string Description => ""; // 物品描述
    }
}