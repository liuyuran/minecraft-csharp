using Base.Interface;
using Base.Items;

namespace Base.Components {
    /// <summary>
    /// 双手正在持有的物品
    /// </summary>
    public class ToolInHand: IComponentData {
        public Item LeftHand = new Hand();
        public Item RightHand = new Hand();
    }
}