using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Base;
using Base.Components;
using Base.Events;
using Base.Interface;
using Base.Manager;

namespace UnitTest.BlackBox;

[TestFixture]
[SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "HeapView.ClosureAllocation")]
public class Tests {
    private Thread? _serverThread;
    private const string NickName = "test1";

    [SetUp]
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

    [TearDown]
    public void Cleanup() {
        Game.Stop();
    }
    
    private static void SendEventToServer(GameEvent @event) {
        CommandTransferManager.NetworkAdapter?.SendToServer(@event);
        Thread.Sleep(500);
    }

    /// <summary>
    /// 多用户登录，检查下发指令
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
        // 登录后退出，检查位置是否成功存储
        SendEventToServer(new PlayerLogoutEvent());
        SendEventToServer(new PlayerJoinEvent { Nickname = NickName });
        Assert.Multiple(() => {
            Assert.That(player.GetComponent<Transform>().Position, Is.EqualTo(new Vector3(1, 1, 1)));
            Assert.That(player.GetComponent<Transform>().Forward, Is.EqualTo(new Vector3(2, 2, 2)));
        });
    }
}