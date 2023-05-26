namespace Server; 

internal static class EntryPoint {
    public static void Main() {
        var server = new Base.Server();
        new Thread(() => server.Start()).Start();
        while (true) {
            var line = Console.ReadLine();
            if (line == "shutdown") {
                break;
            }
        }
    }
}