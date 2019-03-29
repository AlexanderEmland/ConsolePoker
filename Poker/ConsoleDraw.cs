using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public enum TextAlign
    {
        Right,
        Left,
        Center
    }

    public enum AnchorPoint
    {
        None,
        Center,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left,
    }

    public static class ConsoleDraw
    {
        private const int SCREEN_WIDTH = Program.SCREEN_WIDTH;
        private const int SCREEN_HEIGHT = Program.SCREEN_HEIGHT;

        private static Dictionary<AnchorPoint, Point> ScreenPositions = new Dictionary<AnchorPoint, Point>
        {
            { AnchorPoint.Center,      new Point(SCREEN_WIDTH/2, SCREEN_HEIGHT/2) },
            { AnchorPoint.TopLeft,     new Point(0, 0) },
            { AnchorPoint.Top,         new Point(SCREEN_WIDTH/2, 0) },
            { AnchorPoint.TopRight,    new Point(SCREEN_WIDTH - 1, 0) },
            { AnchorPoint.Right,       new Point(SCREEN_WIDTH - 1, SCREEN_HEIGHT/2) },
            { AnchorPoint.BottomRight, new Point(SCREEN_WIDTH - 1, SCREEN_HEIGHT - 1) },
            { AnchorPoint.Bottom,      new Point(SCREEN_WIDTH/2, SCREEN_HEIGHT - 1) },
            { AnchorPoint.BottomLeft,  new Point(0, SCREEN_HEIGHT - 1) },
            { AnchorPoint.Left,        new Point(0, SCREEN_HEIGHT/2) },
        };
        public static TextAlign TextAlign { get; set; }
        public static int SideMargin { get; set; }

        public static ConsoleColor defaultForegroundColor = ConsoleColor.Gray;
        public static ConsoleColor defaultBackgroundColor = ConsoleColor.Black;

        // DRAW A STRING
        public static void String(string s, AnchorPoint anchorPoint)
        {
            String(s, ScreenPositions[anchorPoint]);
        }

        public static void String(string s, AnchorPoint anchorPoint, int yoff)
        {
            String(s, ScreenPositions[anchorPoint] + new Point(0, yoff));
        }
        
        public static void String(string s, int x, int y)
        {
            String(s, new Point(x, y));
        }

        public static void String(string s, Point p)
        {
            int posX = p.X - 0;
            int posY = Math.Max(0, Math.Min(p.Y, SCREEN_HEIGHT));
            if (TextAlign == TextAlign.Left)
            {
                if (posX + s.Length >= SCREEN_WIDTH)
                {
                    posX = SCREEN_WIDTH - s.Length;
                }
            }
            else if (TextAlign == TextAlign.Right)
            {
                if (posX - s.Length < 0)
                {
                    posX = 0;
                }
                else
                {
                    posX = p.X - s.Length+1;
                }
            }
            else if (TextAlign == TextAlign.Center)
            {
                if (posX + s.Length / 2 >= SCREEN_WIDTH)
                {
                    posX = SCREEN_WIDTH - s.Length;
                }
                else if (posX - s.Length / 2 < 0)
                {
                    posX = 0;
                }
                else
                {
                    posX -= s.Length / 2;
                }
            }
            if(posX + s.Length >= SCREEN_WIDTH && p.Y >= SCREEN_HEIGHT - 1) // Special bottom right case

            {
                posX = SCREEN_WIDTH - s.Length - 1;
            }
            if(posX + s.Length >= SCREEN_WIDTH - SideMargin)
            {
                posX = SCREEN_WIDTH - s.Length - SideMargin;
            }
            else if(posX < SideMargin)
            {
                posX = SideMargin;
            }
            Console.SetCursorPosition(posX, posY);
            Console.Write(s);
        }

        // DRAW A CHAR
        public static void Char(char c, Point p)
        {
            Console.SetCursorPosition(p.X, p.Y);
            Console.Write(c);
        }

        public static void Char(char c, int x, int y)
        {
            Char(c, new Point(x, y));
        }

        // DRAW A TEXTBOX
        public static void TextBox(TextBox tb)
        {
            if (tb.GetRowCount() == 0)
                return;

            // Top/left overlap
            int overlapX = tb.Left - SideMargin;
            int overlapY = tb.Top;
            if(overlapX < 0)
            {
                tb.Move(tb.Position.X - overlapX, tb.Position.Y);
            }
            if(overlapY < 0)
            {
                tb.Move(tb.Position.X, tb.Position.Y - overlapY);
            }

            // Bottom/right overlap
            overlapX = tb.Right - (SCREEN_WIDTH - SideMargin);
            overlapY = tb.Bottom - (SCREEN_HEIGHT - 2);
            if(overlapX > 0)
            {
                tb.Move(tb.Right - overlapX, tb.Position.Y);
            }
            if(overlapY > 0)
            {
                tb.Move(tb.Position.X, tb.Bottom - overlapY);
            }

            var strings = tb.GetStrings();
            for (int i = 0; i < strings.Length; i++)
            {
                if(TextAlign == TextAlign.Left)
                {
                    String(strings[i], tb.Left, tb.Top + i);
                }
                else if(TextAlign == TextAlign.Right)
                {
                    if (tb.Left == 0)
                    {
                        String(strings[i], tb.Right - 1, tb.Top + i);
                    }
                    else
                    {
                        String(strings[i], tb.Right, tb.Top + i);
                    }
                }
                else if(TextAlign == TextAlign.Center)
                {
                    String(strings[i], tb.Center.X, tb.Top + i);
                }
            }
        }

        // HELPER FUNCTIONS
        public static string RepeatChar(char c, int n)
        {
            string s = "";

            for (int i = 0; i < n; i++)
            {
                s += c;
            }

            return s;
        }

        // HELPER METHODS
        public static void SetForegroundColor(ConsoleColor c)
        {
            Console.ForegroundColor = c;
        }
        public static void SetBackgroundColor(ConsoleColor c)
        {
            Console.BackgroundColor = c;
        }

        public static void ResetColors()
        {
            SetForegroundColor(defaultForegroundColor);
            SetBackgroundColor(defaultBackgroundColor);
        }

        public static void ResetForegroundColor()
        {
            SetForegroundColor(defaultForegroundColor);
        }

        public static void ResetBackgroundColor()
        {
            SetBackgroundColor(defaultForegroundColor);
        }

        // DEBUG METHODS
        public static void DebugAnchorPoints()
        {
            foreach (var a in ScreenPositions.Keys)
            {
                Point p = ScreenPositions[a];
                Char('x', p);
            }
        }

        public static void DebugTextAlign(string s)
        {
            foreach (var a in ScreenPositions.Keys)
            {
                Point p = ScreenPositions[a];
                String(s, p);
            }
        }
    }
}