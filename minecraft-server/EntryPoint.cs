using System.Numerics;
using Base;
using Base.Components;
using Base.Events;
using Base.Manager;

namespace Server; 

internal static class EntryPoint {
    public static void Main() {
        // Game.Start("", new RemoteNetworkAdapter());
        new Thread(() => Game.Start("default")).Start();
        while (true) {
            Thread.Sleep(100);
            if (CommandTransferManager.NetworkAdapter == null) continue;
            CommandTransferManager.NetworkAdapter.SendToServer(new PlayerJoinEvent {
                Nickname = "test"
            });
            Thread.Sleep(2000);
            CommandTransferManager.NetworkAdapter.SendToServer(new PlayerInfoUpdateEvent {
                Position = new Vector3(4, 4, 4),
                Forward = new Vector3()
            });
            break;
        }

        MessageTypeManager.Instance.FireEvent(new ChatEvent {
            Message = ""
        });
        while (true) {
            var command = Console.ReadLine();
            if (command == "exit") {
                break;
            }
        }
    }
}