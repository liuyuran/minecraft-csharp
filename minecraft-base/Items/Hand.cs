namespace Base.Items {
    public class Hand : Item {
        public override string ID => ""; // 物品id
        public override string Icon => ""; // 物品图标
        public override string Name => ""; // 物品名称
        public override string Description => ""; // 物品描述
        public override int ItemType => 1 << (int)Enums.ItemType.Hand;
    }
}