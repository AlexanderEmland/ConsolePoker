using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Dealer
    {
        public Deck Deck { get; set; }

        public Dealer()
        {
            Deck = new Deck();
        }

        public void Reset()
        {
            Deck.ResetAndShuffle();
        }

        public void Deal(Player p)
        {
            p.HoleCards.Add(Deck.Draw());
        }

        public void Deal(List<Card> cards, int n, bool replaceEmpty = true, bool append = false)
        {
            int addedCards = 0;

            for (int i = 0; i < cards.Count; i++)
            {
                var c = cards.ElementAt(i);
                if (c.Suit == SuitType.None)
                {
                    cards.Insert(cards.IndexOf(c), Deck.Draw());
                    cards.Remove(c);

                    if (++addedCards >= n)
                    {
                        return;
                    }
                }
            }
        }

        public void BurnCard()
        {
            Deck.Draw();
        }
    }
}
