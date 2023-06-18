namespace Base.Blocks {
    public class Air: IBlock {
        public string ID => "classic:air";
        public string Texture => "texture.jpg";
        public string Nbt => "";
        public bool IsAir => true;
    }
}