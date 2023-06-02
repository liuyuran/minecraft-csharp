using System.Collections.Concurrent;
using Base.Utils;

namespace Base.Manager {
    /// <summary>
    /// 作为状态同步和客户端命令输入的缓冲区
    /// </summary>
    public static class CommandTransferManager {
        public static readonly ConcurrentQueue<CommandMessage<Chunk>> ChunkQueue = new();

        public static void UpdateChunkForUser(Chunk chunk, string userId) {
            ChunkQueue.Enqueue(new CommandMessage<Chunk> {
                UserID = userId,
                Message = chunk
            });
        }
    }
}