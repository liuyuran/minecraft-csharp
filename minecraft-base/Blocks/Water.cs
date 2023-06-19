namespace Base.Blocks {
    public class Water: Block {
        public override string ID => "classic:water";
        public override string Texture => "texture.jpg";
        public override bool Transparent => true;
        public override int Resistance => 50;
    }
}