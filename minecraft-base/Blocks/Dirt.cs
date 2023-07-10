using Base.Enums;

namespace Base.Blocks {
    public class Dirt: Block {
        public override string ID => "classic:dirt";
        public override string Icon => "";
        public override string Texture => "classic/dirt.png";
        public override string Name => "泥土";
        public override string Description => "泥土";
        public override BlockType Type => BlockType.Solid;
        public override ItemType DigRequire => Enums.ItemType.Hand;
        public override int HardnessLevel => 1;
        public override int MaxDurability => 1;
    }
}