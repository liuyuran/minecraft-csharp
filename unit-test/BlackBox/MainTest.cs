using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Base;
using Base.Blocks;
using Base.Components;
using Base.Const;
using Base.Events;
using Base.Events.ClientEvent;
using Base.Events.ServerEvent;
using Base.Interface;
using Base.Manager;
using Base.Utils;

namespace UnitTest.BlackBox;

[TestFixture]
[SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "HeapView.ClosureAllocation")]
public class Tests {
    private Thread? _serverThread;
    private const string NickName = "test1";

    [OneTimeSetUp]
    public void Setup() {
        // 启动前清理存档文件
        var assemblyDirectory = $"{TestContext.CurrentContext.TestDirectory}/worlds";
        if (Directory.Exists(assemblyDirectory)) {
            Directory.Delete(assemblyDirectory, true);
        }

        // 启动服务器
        _serverThread = new Thread(() => Game.Start("default"));
        _serverThread.Start();
        Thread.Sleep(500);
    }

    [OneTimeTearDown]
    public void Cleanup() {
        Game.Stop();
    }
    
    private static void SendEventToServer(GameEvent @event) {
        CommandTransferManager.NetworkAdapter?.SendToServer(@event);
        Thread.Sleep(500);
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    [Test, Order(1)]
    public void UserJoin() {
        var uuid = CommandTransferManager.NetworkAdapter?.GetCurrentPlayerUuid();
        Assert.That(uuid, Is.Not.Null);
        if (uuid == null) return;
        SendEventToServer(new PlayerJoinEvent { Nickname = NickName });
        var player = PlayerManager.Instance.GetPlayer(uuid);
        Assert.That(player, Is.Not.Null);
        if (player == null) return;
        // 初次进入游戏，检查玩家昵称
        Assert.That(player.GetComponent<Player>().NickName, Is.EqualTo(NickName));
        // 玩家退出，检查玩家是否被移除
        SendEventToServer(new PlayerLogoutEvent());
        Thread.Sleep(500);
        Assert.That(PlayerManager.Instance.GetPlayer(uuid), Is.Null);
    }

    /// <summary>
    /// 用户移动
    /// </summary>
    [Test, Order(2)]
    public void UserMove() {
        var uuid = CommandTransferManager.NetworkAdapter?.GetCurrentPlayerUuid();
        Assert.That(uuid, Is.Not.Null);
        if (uuid == null) return;
        SendEventToServer(new PlayerJoinEvent { Nickname = NickName });
        // 初次进入游戏，检查玩家初始位置
        var player = PlayerManager.Instance.GetPlayer(uuid);
        Assert.That(player, Is.Not.Null);
        if (player == null) return;
        Assert.Multiple(() => {
            Assert.That(player.GetComponent<Transform>().Position, Is.EqualTo(new Vector3(0, 0, 0)));
            Assert.That(player.GetComponent<Transform>().Forward, Is.EqualTo(new Vector3(0, 0, 0)));
        });
        // 玩家移动，检查玩家位置
        SendEventToServer(new PlayerInfoUpdateEvent {
            Position = new Vector3(1, 1, 1),
            Forward = new Vector3(2, 2, 2)
        });
        Assert.Multiple(() => {
            Assert.That(player.GetComponent<Transform>().Position, Is.EqualTo(new Vector3(1, 1, 1)));
            Assert.That(player.GetComponent<Transform>().Forward, Is.EqualTo(new Vector3(2, 2, 2)));
        });
        // 等待存档
        Thread.Sleep((int)(ParamConst.AutoSaveInterval + 1000));
        // 登录后退出，检查位置是否成功存储
        SendEventToServer(new PlayerLogoutEvent());
        SendEventToServer(new PlayerJoinEvent { Nickname = NickName });
        player = PlayerManager.Instance.GetPlayer(uuid);
        Assert.That(player, Is.Not.Null);
        if (player == null) return;
        Assert.Multiple(() => {
            Assert.That(player.GetComponent<Transform>().Position, Is.EqualTo(new Vector3(1, 1, 1)));
            Assert.That(player.GetComponent<Transform>().Forward, Is.EqualTo(new Vector3(2, 2, 2)));
        });
        SendEventToServer(new PlayerLogoutEvent());
    }

    /// <summary>
    /// 自动掉线
    /// </summary>
    [Test, Order(3)]
    public void AutoDisconnect() {
        SendEventToServer(new PlayerJoinEvent { Nickname = NickName });
        var uuid = CommandTransferManager.NetworkAdapter?.GetCurrentPlayerUuid();
        Assert.That(uuid, Is.Not.Null);
        if (uuid == null) return;
        Thread.Sleep((int)(ParamConst.DisconnectTimeout + 1000));
        var player = PlayerManager.Instance.GetPlayer(uuid);
        Assert.That(player, Is.Null);
    }

    /// <summary>
    /// 挖掘
    /// </summary>
    [Test, Order(4)]
    public void Dig() {
        SendEventToServer(new PlayerJoinEvent { Nickname = NickName });
        while (CommandTransferManager.NetworkAdapter?.TryGetFromServer(out var @event) ?? false) {
            // 只是为了清空一轮传输队列
            if (@event is not ChunkUpdateEvent updateEvent) continue;
            if (updateEvent.Chunk == null) continue;
            if (updateEvent.Chunk.Position != new Vector3(0, 0, -1)) continue;
            Assert.That(updateEvent.Chunk.GetBlock(0 , 0, 0).ID, Is.EqualTo(new Dirt().ID));
        }
        SendEventToServer(new BlockUpdateEvent {
            ActionType = BlockUpdateEvent.ActionTypeEnum.Dig,
            WorldId = 0,
            ChunkPos = new Vector3(0, 0, -1),
            BlockPos = new Vector3(0, 0, 0),
            Direction = Direction.up,
            Operand = ""
        });
        while (CommandTransferManager.NetworkAdapter?.TryGetFromServer(out var @event) ?? false) {
            if (@event is not ChunkUpdateEvent updateEvent) continue;
            if (updateEvent.Chunk == null) continue;
            if (updateEvent.Chunk.Position != new Vector3(0, 0, -1)) continue;
            Assert.That(updateEvent.Chunk.GetBlock(0 , 0, 0).ID, Is.EqualTo(new Air().ID));
        }
        SendEventToServer(new PlayerLogoutEvent());
    }

    /// <summary>
    /// 方块状态保存、掉落与拾取
    /// </summary>
    [Test, Order(5)]
    public void SaveDropAndPick() {
        SendEventToServer(new PlayerJoinEvent { Nickname = NickName });
        string? itemId = null;
        Thread.Sleep(1000);
        while (CommandTransferManager.NetworkAdapter?.TryGetFromServer(out var @event) ?? false)
        {
            if (@event is not ChunkUpdateEvent updateEvent) continue;
            if (updateEvent.Chunk == null) continue;
            if (updateEvent.Chunk.Position != new Vector3(0, 0, -1)) continue;
            Assert.Multiple(() =>
            {
                Assert.That(updateEvent.Chunk.GetBlock(0, 0, 0).ID, Is.EqualTo(new Air().ID));
                Assert.That(updateEvent.Items, Has.Count.EqualTo(1));
            });
            itemId = updateEvent.Items.Values.First().First().ItemID;
        }
        Assert.That(itemId, Is.Not.Null);
        if (itemId == null) return;
        SendEventToServer(new PickUpEvent {
            ItemId = itemId
        });
        var uuid = CommandTransferManager.NetworkAdapter?.GetCurrentPlayerUuid();
        Assert.That(uuid, Is.Not.Null);
        if (uuid == null) return;
        var player = PlayerManager.Instance.GetPlayer(uuid);
        Assert.That(player, Is.Not.Null);
        if (player == null) return;
        var data = player.GetComponent<Inventory>();
        Assert.That(data.Items[0], Is.Not.Null);
        SendEventToServer(new PlayerLogoutEvent());
    }

    /// <summary>
    /// 放置
    /// </summary>
    [Test, Order(6)]
    public void PlaceBlock() {
        SendEventToServer(new PlayerJoinEvent { Nickname = NickName });
        while (CommandTransferManager.NetworkAdapter?.TryGetFromServer(out var @event) ?? false) {
            // 只是为了清空一轮传输队列
            if (@event is not ChunkUpdateEvent updateEvent) continue;
            if (updateEvent.Chunk == null) continue;
            if (updateEvent.Chunk.Position != new Vector3(0, 0, -1)) continue;
            Assert.That(updateEvent.Chunk.GetBlock(0 , 0, 0).ID, Is.EqualTo(new Air().ID));
        }
        SendEventToServer(new SwitchToolEvent {
            isLeft = false,
            InventorySlot = 0
        });
        SendEventToServer(new BlockUpdateEvent {
            ActionType = BlockUpdateEvent.ActionTypeEnum.Active,
            WorldId = 0,
            ChunkPos = new Vector3(0, 0, -1),
            BlockPos = new Vector3(0, 0, 0),
            Direction = Direction.up,
            Operand = ""
        });
        while (CommandTransferManager.NetworkAdapter?.TryGetFromServer(out var @event) ?? false) {
            if (@event is not ChunkUpdateEvent updateEvent) continue;
            if (updateEvent.Chunk == null) continue;
            if (updateEvent.Chunk.Position != new Vector3(0, 0, -1)) continue;
            Assert.That(updateEvent.Chunk.GetBlock(0 , 0, 1).ID, Is.EqualTo(new Dirt().ID));
        }
        SendEventToServer(new PlayerLogoutEvent());
    }
    
    /// <summary>
    /// 交互
    /// </summary>
    [Test, Order(7)]
    public void Action() { }

    /// <summary>
    /// 聊天
    /// </summary>
    [Test, Order(8)]
    public void Chat() {
        var uuid = CommandTransferManager.NetworkAdapter?.GetCurrentPlayerUuid();
        Assert.That(uuid, Is.Not.Null);
        if (uuid == null) return;
        SendEventToServer(new PlayerJoinEvent { Nickname = NickName });
        var player = PlayerManager.Instance.GetPlayer(uuid);
        Assert.That(player, Is.Not.Null);
        if (player == null) return;
        SendEventToServer(new ChatEvent { Message = "test" });
        var haveChatMessage = false;
        while (CommandTransferManager.NetworkAdapter?.TryGetFromServer(out var @event) ?? false) {
            if (@event is not ChatEvent chatEvent) continue;
            Assert.That(chatEvent.Message, Is.EqualTo($"[{NickName}]: test"));
            haveChatMessage = true;
            break;
        }
        Assert.That(haveChatMessage, Is.True);
    }
}