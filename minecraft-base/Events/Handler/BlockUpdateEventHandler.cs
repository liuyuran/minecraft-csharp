using System;
using Base.Blocks;
using Base.Components;
using Base.Const;
using Base.Interface;
using Base.Items;
using Base.Manager;
using Base.Utils;

namespace Base.Events.Handler {
    /// <summary>
    /// 处理客户端传递过来的对方块的操作
    /// </summary>
    public class BlockUpdateEventHandler : IGameEventHandler<BlockUpdateEvent> {
        public void Run(BlockUpdateEvent e) {
            switch (e.ActionType) {
                case BlockUpdateEvent.ActionTypeEnum.Dig:
                    // 破坏方块
                    var blockPos = e.BlockPos;
                    var chunkPos = e.ChunkPos;
                    var chunk = ChunkManager.Instance.GetChunk(e.WorldId, chunkPos);
                    // 忽略通过特殊手段远程交互的情况
                    var block = chunk?.GetBlockCrossChunk((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);
                    if (block == null) return;
                    var player = PlayerManager.Instance.GetPlayer(e.UserID);
                    var rightTool = player.GetComponent<ToolInHand>().Right;
                    // 忽略工具不对的情况
                    if (!rightTool.CanDig(block)) return;
                    // 忽略不可破坏方块
                    if (block.MaxDurability < 0) return;
                    block.Durability += 10;
                    if (block.Durability > block.MaxDurability) {
                        chunk?.SetBlockCrossChunk((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z, new Air());
                    }

                    if (rightTool.MaxDurability > 0) {
                        // 如果工具有耐久度，减少耐久度
                        rightTool.Durability -= 1;
                        if (rightTool.Durability <= 0) {
                            // 工具破碎，替换为手
                            player.GetComponent<ToolInHand>().Right = new Hand();
                        }
                    }
                    EventBus.OnBlockUpdateEvent(block);

                    break;
                case BlockUpdateEvent.ActionTypeEnum.Action:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}