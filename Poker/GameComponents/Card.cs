using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Poker
{
    public enum SuitType
    {
        Spade,
        Club,
        Heart,
        Diamond,
        None
    }

    public class Card
    {
        public SuitType Suit { get; set; }
        public int Value { get; set; }

        private static readonly char borderTopBottom = '─';
        private static readonly char borderLeftRight = '│';
        private static readonly char borderTopLeft = '┌';
        private static readonly char borderTopRight = '┐';
        private static readonly char borderBottomLeft = '└';
        private static readonly char borderBottomRight = '┘';

        private static readonly Dictionary<SuitType, char> suitChars = new Dictionary<SuitType, char>
        {
            { SuitType.Spade,   '♠' },
            { SuitType.Heart,   '♥' },
            { SuitType.Club,    '♣' },
            { SuitType.Diamond, '♦' },
            { SuitType.None,    ' ' },
        };

        public Card(int value, SuitType suit)
        {
            Value = value;
            Suit = suit;
        }

        public override string ToString()
        {
            char suitChar = suitChars[Suit];
            return GetValueString() + suitChar;
        }

        private string GetValueString()
        {
            switch (Value)
            {
                case 0: return " ";
                case 1: return "A";
                case 11: return "J";
                case 12: return "Q";
                case 13: return "K";
                default: return Value.ToString();
            }
        }

        public void DrawBig(int x, int y, int w, int h)
        {
            int yOffset = 0;
            int valueW = GetValueString().Length;
            int actualW = valueW > 1 && h < 3 ? w + 1 : w;
            Console.SetCursorPosition(x, y);

            // Draw top of card
            DrawUpperBorder(actualW);
            yOffset++;

            //Draw middle section of card
            for (int i = 0; i < h; i++)
            {
                Console.SetCursorPosition(x, y + yOffset);
                Console.Write(borderLeftRight);

                //Draw left side of card face
                if (i == 0)
                {
                    DrawValue();
                }
                else if (i == 1)
                {
                    DrawSuit();
                    if (Value == 10)
                    {
                        DrawRepeated(' ', 1);
                    }
                }
                else if (Value == 10 && h > 2)
                {
                    DrawRepeated(' ', 2);
                }
                else
                {
                    DrawRepeated(' ', 1);
                }

                DrawRepeated(' ', actualW - GetValueString().Length * 2);
                if (Value == 10 && i > 0 && i < h - 1 && h < 3)
                    DrawRepeated(' ', 1);

                //Draw right side of card face
                if (i == h - 1)
                {
                    DrawValue();
                }
                else if (i == h - 2)
                {
                    if (Value == 10)
                    {
                        DrawRepeated(' ', 1);
                    }
                    DrawSuit();
                }
                else if (Value == 10 && h > 2)
                {
                    DrawRepeated(' ', 2);
                }
                else
                {
                    DrawRepeated(' ', 1);
                }

                Console.Write(borderLeftRight);
                yOffset++;
            }

            //Draw bottom of card
            Console.SetCursorPosition(x, y + yOffset);
            DrawLowerBorder(actualW);
        }

        public void DrawBigHidden(int x, int y, int w, int h)
        {
            int yOffset = 0;
            Console.SetCursorPosition(x, y);

            // Draw top of card
            DrawUpperBorder(w);
            yOffset++;

            //Draw middle section of card
            for (int i = 0; i < h; i++)
            {
                Console.SetCursorPosition(x, y + yOffset);
                Console.Write(borderLeftRight);

                DrawPattern('/', w, 3, i);

                Console.Write(borderLeftRight);
                yOffset++;
            }

            //Draw bottom of card
            Console.SetCursorPosition(x, y + yOffset);
            DrawLowerBorder(w);
        }

        public void DrawMiniShown(int x, int y)
        {
            int yOffset = 0;
            Console.SetCursorPosition(x, y + yOffset);
            DrawUpperBorder(2);
            yOffset++;

            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(x, y + yOffset);
                Console.Write(borderLeftRight);
                if (Value == 10)
                {
                    if (i == 0)
                    {
                        DrawValue();
                    }
                    else
                    {
                        DrawSuit();
                        DrawSuit();
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        DrawValue();
                        DrawSuit();
                    }
                    else
                    {
                        DrawSuit();
                        DrawValue();
                    }
                }
                Console.Write(borderLeftRight);
                yOffset++;
            }
            Console.SetCursorPosition(x, y + yOffset);
            DrawLowerBorder(2);
        }

        public void DrawMiniHidden(int x, int y)
        {
            int yOffset = 0;
            Console.SetCursorPosition(x, y + yOffset++);

            DrawUpperBorder(2);
            Console.SetCursorPosition(x, y + yOffset++);
            Console.Write(borderLeftRight);
            DrawPattern('/', 2, 1, 0);
            Console.Write(borderLeftRight);
            Console.SetCursorPosition(x, y + yOffset++);
            Console.Write(borderLeftRight);
            DrawPattern('/', 2, 1, 0);
            Console.Write(borderLeftRight);
            Console.SetCursorPosition(x, y + yOffset++);
            DrawLowerBorder(2);
        }

        public void DrawMiniFolded(int x, int y)
        {
            int yOffset = 0;
            Console.SetCursorPosition(x, y + yOffset++);

            DrawUpperBorder(2);
            Console.SetCursorPosition(x, y + yOffset++);
            Console.Write(borderLeftRight);
            Console.Write('\\');
            Console.Write('/');
            Console.Write(borderLeftRight);
            Console.SetCursorPosition(x, y + yOffset++);
            Console.Write(borderLeftRight);
            Console.Write('/');
            Console.Write('\\');
            Console.Write(borderLeftRight);
            Console.SetCursorPosition(x, y + yOffset++);
            DrawLowerBorder(2);
        }

        private void DrawUpperBorder(int w)
        {
            Console.Write(borderTopLeft);
            DrawRepeated(borderTopBottom, w);
            Console.Write(borderTopRight);
        }

        private void DrawLowerBorder(int w)
        {
            Console.Write(borderBottomLeft);
            DrawRepeated(borderTopBottom, w);
            Console.Write(borderBottomRight);
        }

        private void DrawPattern(char c, int n, int spacing, int offset)
        {
            for (int i = 0; i < n; i++)
            {
                if ((i + offset) % spacing == 0)
                    Console.Write(c);
                else
                    Console.Write(" ");
            }
        }

        private void DrawRepeated(char c, int n)
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write(c);
            }
        }

        private void DrawValue()
        {
            Console.Write(GetValueString());
        }

        private void DrawSuit()
        {
            if (Suit == SuitType.Diamond || Suit == SuitType.Heart)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(suitChars[Suit]);
            Console.ResetColor();
        }
    }
}
