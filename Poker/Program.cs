using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace Poker
{
    class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        public const int SCREEN_WIDTH = 150; //150
        public const int SCREEN_HEIGHT = 40; //36

        static void Main(string[] args)
        {
            /* Fix console size */
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            SetConsoleSize(SCREEN_WIDTH, SCREEN_HEIGHT);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }

            Console.OutputEncoding = Encoding.Unicode;
            Console.Write("Welcome to [PokerName]\nNumber of opponents > ");

            int opponentCount = 1;
            Console.CursorVisible = false;
            char key;
            do
            {
                key = Console.ReadKey(true).KeyChar;
            } while (!char.IsNumber(key) || key == '0');

            opponentCount = Convert.ToInt32(key.ToString());
            Console.WriteLine(opponentCount);
            Game game = new Game(opponentCount);
            Console.ReadKey();
        }

        private static void SetConsoleSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
            Console.BufferHeight = Console.WindowHeight;
        }
    }
}
