using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
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
        public Point Center {
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
            return new Point(Left + Width/2, Top + Height/2);
        }
    }
}
