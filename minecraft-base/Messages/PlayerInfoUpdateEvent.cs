using System.Numerics;
using Base.Interface;

namespace Base.Messages {
    public class PlayerInfoUpdateEvent: GameEvent {
        public Vector3 Position;
        public Vector3 Forward;
    }
}