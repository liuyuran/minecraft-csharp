using System.Collections.Concurrent;
using Base.Messages;
using Base.Utils;

namespace Base.Manager {
    /// <summary>
    /// 作为状态同步和客户端命令输入的缓冲区
    /// </summary>
    public static class CommandTransferManager {
        public static readonly ConcurrentQueue<CommandMessage<Chunk>> ChunkOutQueue = new();
        public static readonly ConcurrentQueue<CommandMessage<UserLogin>> LoginInQueue = new();
        public static readonly ConcurrentQueue<CommandMessage<int>> LogoutInQueue = new();

        public static void UpdateChunkForUser(Chunk chunk, string userId) {
            ChunkOutQueue.Enqueue(new CommandMessage<Chunk> {
                UserID = userId,
                Message = chunk
            });
        }

        public static string AddUser(string nickname) {
            var uuid = System.Guid.NewGuid().ToString();
            LoginInQueue.Enqueue(new CommandMessage<UserLogin> {
                UserID = uuid,
                Message = new UserLogin {
                    Nickname = nickname
                }
            });
            return uuid;
        }
        
        public static string RemoveUser(string nickname) {
            var uuid = System.Guid.NewGuid().ToString();
            LogoutInQueue.Enqueue(new CommandMessage<int> {
                UserID = uuid,
                Message = 0
            });
            return uuid;
        }
    }
}