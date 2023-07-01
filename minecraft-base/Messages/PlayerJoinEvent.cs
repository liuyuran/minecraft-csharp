using Base.Interface;

namespace Base.Messages {
    public class PlayerJoinEvent: GameEvent {
        public string Nickname = "";
    }
}