using System.Numerics;
using Base.Interface;

namespace Base.Events {
    public class PlayerInfoUpdateEvent: GameEvent {
        public Vector3 Position;
        public Vector3 Forward;
    }
}