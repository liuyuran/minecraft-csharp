using System;

namespace Base.Manager {
    public class LogManager {
        public static LogManager Instance { get; } = new();

        private static void WriteLine(string message, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void ReWriteLine(string message, ConsoleColor color) {
            var currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
            WriteLine(message, color);
        }

        public void Debug(string message, bool rewrite = false) {
            if (rewrite)
                ReWriteLine(message, ConsoleColor.DarkGray);
            else
                WriteLine(message, ConsoleColor.DarkGray);
        }

        public void Info(string message, bool rewrite = false) {
            if (rewrite)
                ReWriteLine(message, ConsoleColor.Gray);
            else
                WriteLine(message, ConsoleColor.Gray);
        }

        public void Warning(string message, bool rewrite = false) {
            if (rewrite)
                ReWriteLine(message, ConsoleColor.Yellow);
            else
                WriteLine(message, ConsoleColor.Yellow);
        }

        public void Error(string message, bool rewrite = false) {
            if (rewrite)
                ReWriteLine(message, ConsoleColor.Red);
            else
                WriteLine(message, ConsoleColor.Red);
        }
    }
}