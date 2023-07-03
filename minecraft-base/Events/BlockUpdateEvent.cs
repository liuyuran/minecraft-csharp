using System.Numerics;
using Base.Interface;
using Base.Utils;

namespace Base.Events {
    public class BlockUpdateEvent: GameEvent {
        public enum ActionTypeEnum {
            Dig = 0,
            Action = 1,
        }
        public Vector3 ChunkPos = Vector3.Zero;
        public Vector3 BlockPos = Vector3.Zero;
        public Direction Direction = Direction.up;
        public ActionTypeEnum ActionType = 0;
        public string Operand = "";
    }
}