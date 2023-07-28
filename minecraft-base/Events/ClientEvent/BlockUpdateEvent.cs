﻿using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Base.Interface;
using Base.Utils;

namespace Base.Events.ClientEvent {
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    [SuppressMessage("ReSharper", "ConvertToConstant.Global")]
    public class BlockUpdateEvent: GameEvent {
        public enum ActionTypeEnum {
            Dig = 0,
            Active = 1,
        }
        public int WorldId = 0;
        public Vector3 ChunkPos = Vector3.Zero;
        public Vector3 BlockPos = Vector3.Zero;
        public Direction Direction = Direction.up;
        public ActionTypeEnum ActionType = 0;
        public string Operand = "";
    }
}