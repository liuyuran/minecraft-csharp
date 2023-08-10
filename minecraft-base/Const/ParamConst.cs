﻿namespace Base.Const {
    public static class ParamConst {
        /// <summary>
        /// 存档基准路径
        /// </summary>
        public const string ArchiveBasePath = "worlds";
        /// <summary>
        /// 视野距离
        /// </summary>
        public const ushort DisplayDistance = 2;
        public const ushort DisplayDistanceY = 1;
        /// <summary>
        /// 区块边长
        /// </summary>
        public const int ChunkSize = 10;
        /// <summary>
        /// 状态同步最小间隔
        /// </summary>
        public const long SyncInterval = 500;
        /// <summary>
        /// 掉线延时
        /// </summary>
        public const long DisconnectTimeout = 10000;
        /// <summary>
        /// 自动存档间隔
        /// </summary>
        public const long AutoSaveInterval = 5000;
        /// <summary>
        /// 区块自动卸载延时
        /// </summary>
        public const long ChunkUnloadDelay = 10000;
    }
}