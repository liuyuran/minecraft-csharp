namespace Base.Blocks {
    public class Water: Block {
        public override string ID => "classic:water";
        public override string Texture => "texture.jpg";
        public override string Name => "水";
        public override bool Transparent => false;
        public override int Resistance => 50;
        public override BlockType Type => BlockType.Liquid;
        public override int HardnessLevel => 1;
        public override int MaxDurability => 1;
    }
}