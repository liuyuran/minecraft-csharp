using System.Numerics;
using Base.Interface;

namespace Base.Events.ClientEvent {
    public class PlayerInfoUpdateEvent: GameEvent {
        public Vector3 Position;
        public Vector3 Forward;
    }
}