using Base.Interface;

namespace Base.Components {
    public struct Position: IComponentData {
        public long X;
        public long Y;
        public long Z;
    }
}