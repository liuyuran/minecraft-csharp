using Base;
using Base.Manager;

namespace Server; 

internal static class EntryPoint {
    public static void Main() {
        // Game.Start("", new RemoteNetworkAdapter());
        new Thread(() => Game.Start("default")).Start();
        while (true) {
            Thread.Sleep(100);
            if (CommandTransferManager.NetworkAdapter == null) continue;
            CommandTransferManager.NetworkAdapter?.JoinGame("test");
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