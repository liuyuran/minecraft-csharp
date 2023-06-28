namespace Base.Blocks {
    public class Stone: Block {
        public override string ID => "classic:bedrock";
        public override string Icon => "";
        public override string Texture => "texture.jpg";
        public override string Name => "石头";
        public override string Description => "石头";
        public override BlockType Type => BlockType.Solid;
        public override int HardnessLevel => 2;
        public override int MaxDurability => 30;
    }
}