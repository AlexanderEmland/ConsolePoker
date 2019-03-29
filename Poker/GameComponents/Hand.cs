using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Hand
    {
        public List<Card> Cards { get; set; }
        public int Strength { get; set; }

        public Hand()
        {
            Cards = new List<Card>();
        }

        public void Reset()
        {
            Cards.Clear();
        }

        public void Add(Card c)
        {
            Cards.Add(c);
        }

        public void Draw(int x, int y, bool hidden = true, bool folded = false, bool reversed = false)
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                var card = Cards.ElementAt(i);
                int xOffset = -i * 4;
                int yOffset = reversed ? -3 : 0;

                if (hidden)
                {
                    card.DrawMiniHidden(x + xOffset, y + yOffset);
                }
                else if (folded)
                {
                    card.DrawMiniFolded(x + xOffset, y + yOffset);
                }
                else
                {
                    card.DrawMiniShown(x + xOffset, y + yOffset);
                }
            }
        }

        public override string ToString()
        {
            string output = "";
            foreach (Card c in Cards)
            {
                output += c.ToString() + " ";
            }
            return "";
        }
    }
}
