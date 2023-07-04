using Base.Interface;
using Base.Items;

namespace Base.Components {
    /// <summary>
    /// 双手正在持有的物品
    /// </summary>
    public class ToolInHand: IComponentData {
        public Item Left = new Hand();
        public Item Right = new Hand();
    }
}