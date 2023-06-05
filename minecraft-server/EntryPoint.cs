using Base;
using Base.NetworkAdapters;

namespace Server; 

internal static class EntryPoint {
    public static void Main() {
        Game.Start("", new RemoteNetworkAdapter());
    }
}