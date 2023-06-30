using ProtoBuf;

namespace Base.Interface {
    [ProtoContract]
    public abstract class GameEvent {
        public string UserID = "";
    }
}