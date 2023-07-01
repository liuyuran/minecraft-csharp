using System.Numerics;
using Base.Interface;

namespace Base.Components {
    public struct Transform: IComponentData {
        public Vector3 Position;
        public Vector3 Forward;
    }
}