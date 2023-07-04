using Base.Enums;

namespace Base.Blocks {
    public class Air: Block {
        public override string ID => "classic:air";
        public override string Icon => "";
        public override string Texture => "texture.jpg";
        public override string Name => "空气";
        public override bool Transparent => true;
        public override BlockType Type => BlockType.Air;
    }
}