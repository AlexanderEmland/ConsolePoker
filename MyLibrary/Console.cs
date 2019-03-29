using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace MyLibrary.Console
{
    public static class Constants
    {
        public static int SCREEN_WIDTH
        {
            get
            {
                int width = 0;
                try
                {
                    width = int.Parse(ConfigurationManager.AppSettings.Get("width"));
                }
                catch (Exception e)
                {
                    width = 150;
                }
                return width;
            }
        }
        public static int SCREEN_HEIGHT
        {
            get
            {
                int height = 0;
                try
                {
                    height = int.Parse(ConfigurationManager.AppSettings.Get("height"));
                }
                catch (Exception e)
                {
                    height = 40;
                }
                return height;
            }
        }
    }

    namespace Drawing
    {
        public struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static Point operator +(Point a, Point b)
            {
                return new Point(a.X + b.X, a.Y + b.Y);
            }
        }

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

        public static class Draw
        {
            public static readonly int SCREEN_WIDTH = Constants.SCREEN_WIDTH;
            public static readonly int SCREEN_HEIGHT = Constants.SCREEN_HEIGHT - 1;

            private static Dictionary<AnchorPoint, Point> ScreenPositions = new Dictionary<AnchorPoint, Point>
            {
                { AnchorPoint.Center,      new Point(SCREEN_WIDTH/2, SCREEN_HEIGHT/2) },
                { AnchorPoint.TopLeft,     new Point(0, 0) },
                { AnchorPoint.Top,         new Point(SCREEN_WIDTH/2, 0) },
                { AnchorPoint.TopRight,    new Point(SCREEN_WIDTH - 1, 0) },
                { AnchorPoint.Right,       new Point(SCREEN_WIDTH - 1, SCREEN_HEIGHT/2) },
                { AnchorPoint.BottomRight, new Point(SCREEN_WIDTH - 1, SCREEN_HEIGHT) },
                { AnchorPoint.Bottom,      new Point(SCREEN_WIDTH/2, SCREEN_HEIGHT) },
                { AnchorPoint.BottomLeft,  new Point(0, SCREEN_HEIGHT) },
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
                        posX = p.X - s.Length + 1;
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
                if (posX + s.Length >= SCREEN_WIDTH && p.Y >= SCREEN_HEIGHT - 1) // Special bottom right case

                {
                    posX = SCREEN_WIDTH - s.Length - 1;
                }
                if (posX + s.Length >= SCREEN_WIDTH - SideMargin)
                {
                    posX = SCREEN_WIDTH - s.Length - SideMargin;
                }
                else if (posX < SideMargin)
                {
                    posX = SideMargin;
                }
                System.Console.SetCursorPosition(posX, posY);
                System.Console.Write(s);
            }

            // DRAW A CHAR
            public static void Char(char c, Point p)
            {
                System.Console.SetCursorPosition(p.X, p.Y);
                System.Console.Write(c);
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
                if (overlapX < 0)
                {
                    tb.Move(tb.Position.X - overlapX, tb.Position.Y);
                }
                if (overlapY < 0)
                {
                    tb.Move(tb.Position.X, tb.Position.Y - overlapY);
                }

                // Bottom/right overlap
                overlapX = tb.Right - (SCREEN_WIDTH - SideMargin);
                overlapY = tb.Bottom - (SCREEN_HEIGHT - 2);
                if (overlapX > 0)
                {
                    tb.Move(tb.Right - overlapX, tb.Position.Y);
                }
                if (overlapY > 0)
                {
                    tb.Move(tb.Position.X, tb.Bottom - overlapY);
                }

                var strings = tb.GetStrings();
                for (int i = 0; i < strings.Length; i++)
                {
                    if (TextAlign == TextAlign.Left)
                    {
                        String(strings[i], tb.Left, tb.Top + i);
                    }
                    else if (TextAlign == TextAlign.Right)
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
                    else if (TextAlign == TextAlign.Center)
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
                System.Console.ForegroundColor = c;
            }
            public static void SetBackgroundColor(ConsoleColor c)
            {
                System.Console.BackgroundColor = c;
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

        public enum VerticalAlignment
        {
            //Inherit,
            Top,
            Center,
            Bottom
        }

        public enum HorizontalAlignment
        {
            //Inherit,
            Left,
            Center,
            Right
        }

        public class TextBox
        {
            public VerticalAlignment VerticalAlignment { get; set; }
            public HorizontalAlignment HorizontalAlignment { get; set; }
            public int Height
            {
                get
                {
                    return strings.Count;
                }
            }
            public int Width
            {
                get
                {
                    return strings.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
                }
            }
            public int Top
            {
                get
                {
                    return CalculateTop();
                }
            }
            public int Right
            {
                get
                {
                    return CalculateRight();
                }
            }
            public int Bottom
            {
                get
                {
                    return CalculateBottom();
                }
            }
            public int Left
            {
                get
                {
                    return CalculateLeft();
                }
            }
            public Point Center
            {
                get
                {
                    return CalculateCenter();
                }
            }
            public Point Position { get; set; }

            private List<string> strings;

            public TextBox()
            {
                Position = new Point(0, 0);
                strings = new List<string>();
            }

            //public TextBox(Point p)
            //{
            //    Position = p;
            //    strings = new List<string>();
            //}

            public string[] GetStrings()
            {
                return strings.ToArray();
            }

            public void AddString(string s)
            {
                strings.Add(s);
            }

            public void Move(int x, int y)
            {
                Move(new Point(x, y));
            }

            public void Move(Point p)
            {
                Position = p;
            }

            public int GetRowCount()
            {
                return Height;
            }

            private int CalculateTop()
            {
                switch (VerticalAlignment)
                {
                    //case VerticalAlignment.Center:
                    //    return Position.Y - Height / 2;
                    //case VerticalAlignment.Bottom:
                    //    return Position.Y - Height;
                    //case VerticalAlignment.Top:
                    //default:
                    //    return Position.Y;
                    case VerticalAlignment.Center:
                        return Position.Y - Height / 2;
                    case VerticalAlignment.Top:
                        return Position.Y - Height;
                    case VerticalAlignment.Bottom:
                    default:
                        return Position.Y;
                }
            }

            private int CalculateLeft()
            {
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Center:
                        return Position.X - Width / 2;
                    case HorizontalAlignment.Left:
                        return Position.X - Width;
                    case HorizontalAlignment.Right:
                    default:
                        return Position.X;
                }
            }

            private int CalculateBottom()
            {
                return Top + Height;
            }

            private int CalculateRight()
            {
                return Left + Width;
            }

            private Point CalculateCenter()
            {
                return new Point(Left + Width / 2, Top + Height / 2);
            }
        }

    }

    namespace Utils
    {
        public static class Text
        {
            public static string RepeatChar(char c, int n)
            {
                string output = "";
                for (int i = 0; i < n; i++)
                {
                    output += c;
                }
                return output;
            }
        }

        public static class Input
        {
            public static void GetNumericInput()
            {
                char key;
                do
                {
                    key = System.Console.ReadKey(true).KeyChar;
                } while (true);
            }
        }
    }
}
