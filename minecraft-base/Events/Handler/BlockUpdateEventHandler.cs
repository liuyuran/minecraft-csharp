using System;
using System.Numerics;
using Base.Blocks;
using Base.Components;
using Base.Const;
using Base.Enums;
using Base.Events.InnerBus;
using Base.Interface;
using Base.Items;
using Base.Manager;
using Base.Utils;

namespace Base.Events.Handler {
    /// <summary>
    /// 处理客户端传递过来的对方块的操作
    /// </summary>
    public class BlockUpdateEventHandler : IGameEventHandler<BlockUpdateEvent> {
        public BlockUpdateEventHandler() {
            EventBus.Instance.BlockBreakEvent += @event => {
                var random = new Random();
                var dropItems = @event.Block.DropItems;
                foreach (var dropItem in dropItems) {
                    var loot = random.NextDouble();
                    if (loot > dropItem.Weight) continue;
                    var item = EntityManager.Instance.Instantiate();
                    item.AddComponent(new DroppedItem {
                        ItemID = dropItem.Item.ID
                    });
                    item.AddComponent(new Transform {
                        Forward = Vector3.Zero,
                        Position = new Vector3(
                            @event.ChunkPos.X * ParamConst.ChunkSize + @event.BlockPos.X,
                            @event.ChunkPos.Y * ParamConst.ChunkSize + @event.BlockPos.Y,
                            @event.ChunkPos.Z * ParamConst.ChunkSize + @event.BlockPos.Z
                        )
                    });
                }
            };
            EventBus.Instance.ItemUsedEvent += @event => {
                // TODO 放置方块
                if ((@event.Item.ItemType & (int)ItemType.Block) > 0) {
                    var targetPos = @event.BlockPos;
                    switch (@event.Direction) {
                        case Direction.north:
                            targetPos.Z++;
                            break;
                        case Direction.south:
                            targetPos.Z--;
                            break;
                        case Direction.east:
                            targetPos.X++;
                            break;
                        case Direction.west:
                            targetPos.X--;
                            break;
                        case Direction.up:
                            targetPos.Y++;
                            break;
                        case Direction.down:
                            targetPos.Y--;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    var chunkPos = @event.ChunkPos;
                    Chunk.NormallyBlockPos(ref targetPos, ref chunkPos);
                    var chunk = ChunkManager.Instance.GetChunk(@event.WorldId, chunkPos);
                    var block = chunk?.GetBlockCrossChunk((int)targetPos.X, (int)targetPos.Y, (int)targetPos.Z);
                    if (block == null) return;
                    if (block.Type == BlockType.Solid) return; // 固体不可覆盖
                    chunk?.SetBlockCrossChunk(targetPos, @event.Block);
                }
            };
        }

        public void Run(BlockUpdateEvent e) {
            switch (e.ActionType) {
                case BlockUpdateEvent.ActionTypeEnum.Dig:
                    // 破坏方块
                    DigAction(e);
                    break;
                case BlockUpdateEvent.ActionTypeEnum.Active:
                    // 使用手中的工具，或激活方块
                    ActiveAction(e);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private static void DigAction(BlockUpdateEvent e) {
            var blockPos = e.BlockPos;
            var chunkPos = e.ChunkPos;
            Chunk.NormallyBlockPos(ref blockPos, ref chunkPos);
            var chunk = ChunkManager.Instance.GetChunk(e.WorldId, chunkPos);
            // 忽略通过特殊手段远程交互的情况
            var block = chunk?.GetBlockCrossChunk((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);
            if (block == null) return;
            var player = PlayerManager.Instance.GetPlayer(e.UserID);
            // 触发打击事件
            EventBus.Instance.OnBlockHitEvent(new BlockHitEvent {
                UserId = e.UserID,
                Block = block,
                BlockPos = blockPos,
                ChunkPos = chunkPos,
                WorldId = e.WorldId
            });
            var rightTool = player.GetComponent<ToolInHand>().Right;
            // 忽略工具不对的情况
            if (!rightTool.CanDig(block)) return;
            // 忽略不可破坏方块
            if (block.MaxDurability < 0) return;
            block.Durability += 10;
            if (block.Durability > block.MaxDurability) {
                // 挖掘成功，替换方块数据，并触发事件
                chunk?.SetBlockCrossChunk(blockPos, new Air());
                EventBus.Instance.OnBlockBreakEvent(new BlockBreakEvent {
                    UserId = e.UserID,
                    Block = block,
                    BlockPos = blockPos,
                    ChunkPos = chunkPos,
                    WorldId = e.WorldId
                });
            }

            if (rightTool.MaxDurability > 0) {
                // 如果工具有耐久度，减少耐久度
                rightTool.Durability -= 1;
                if (rightTool.Durability <= 0) {
                    // 工具破碎，替换为手
                    player.GetComponent<ToolInHand>().Right = new Hand();
                }
            }
        }

        private static void ActiveAction(BlockUpdateEvent e) {
            var blockPos = e.BlockPos;
            var chunkPos = e.ChunkPos;
            Chunk.NormallyBlockPos(ref blockPos, ref chunkPos);
            var chunk = ChunkManager.Instance.GetChunk(e.WorldId, chunkPos);
            // 忽略通过特殊手段远程交互的情况
            var block = chunk?.GetBlockCrossChunk((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);
            if (block == null) return;
            var player = PlayerManager.Instance.GetPlayer(e.UserID);
            var rightTool = player.GetComponent<ToolInHand>().Right;
            EventBus.Instance.OnItemUsedEvent(new ItemUsedEvent {
                UserId = e.UserID,
                Block = block,
                BlockPos = blockPos,
                ChunkPos = chunkPos,
                WorldId = e.WorldId,
                Direction = e.Direction,
                Item = rightTool
            });
        }
    }
}