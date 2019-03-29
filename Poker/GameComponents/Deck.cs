using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Deck
    {
        private Stack<Card> _cards;
        private static Random rng = new Random();

        public Deck()
        {
            _cards = new Stack<Card>();
            ResetAndShuffle();
        }

        public void ResetAndShuffle()
        {
            GenerateNewCards();
            Shuffle();
        }

        private void Shuffle()
        {
            _cards = new Stack<Card>(_cards.OrderBy(x => rng.Next()));
        }

        private void GenerateNewCards()
        {
            _cards.Clear();
            for (int i = 0; i < 52; i++)
            {
                int cardValue = i % 13 + 1;
                SuitType cardSuit = (SuitType)(i % 4);
                _cards.Push(new Card(cardValue, cardSuit));
            }
        }

        public int GetSize()
        {
            return _cards.Count;
        }

        public Card Draw()
        {
            return _cards.Pop();
        }
    }
}
