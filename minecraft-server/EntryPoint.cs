using System.Numerics;
using Base;
using Base.Components;
using Base.Manager;

namespace Server; 

internal static class EntryPoint {
    public static void Main() {
        // Game.Start("", new RemoteNetworkAdapter());
        new Thread(() => Game.Start("default")).Start();
        while (true) {
            Thread.Sleep(100);
            if (CommandTransferManager.NetworkAdapter == null) continue;
            CommandTransferManager.NetworkAdapter.JoinGame("test");
            Thread.Sleep(2000);
            CommandTransferManager.NetworkAdapter.UpdatePlayerInfo(new Position {
                X = 4,
                Y = 4,
                Z = 4
            }, new Position());
            break;
        }
        while (true) {
            var command = Console.ReadLine();
            if (command == "exit") {
                break;
            }
        }
    }
}