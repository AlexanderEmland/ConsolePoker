using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public enum Status
    {
        None,
        Checked,
        Called,
        Bet,
        Raised,
        AllIn,
        Folded
    }
    class Player
    {
        public Hand HoleCards { get; set; }
        public int HandStrength { get; set; }
        public int CurrentMoney { get; set; }
        public int CurrentBet { get; set; }
        public bool HasSmallBlind { get; set; }
        public bool HasBigBlind { get; set; }
        public bool IsCurrentPlayer { get; set; }
        //public bool HasFolded { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }

        protected Game game;

        public Player(string name, Game game)
        {
            CurrentMoney = 1000;
            Name = name;
            HoleCards = new Hand();
            this.game = game;
            //HasBigBlind = true;
            Reset();
        }

        // PLAYER LOGIC
        public virtual void MakeDecision(int maximumBet)
        {
            Status = Status.Called;
            // Display available options
            // Take input from the player here
        }

        public void Check()
        {
            // Basically do nothing
            Status = Status.Checked;

        }

        public void Call(int maximumBet)
        {
            // Match the maximum bet
            CurrentBet = maximumBet - CurrentBet;
            if (CurrentBet >= CurrentMoney)
            {
                Status = Status.AllIn;
                CurrentBet = CurrentMoney;
            }
            else
                Status = Status.Called;
        }

        public void Bet(int amount)
        {
            // Display number input
            // Bet 
            CurrentBet = Math.Min(amount, CurrentMoney);
            Status = CurrentBet == CurrentMoney
                   ? Status.AllIn
                   : Status.Bet;
            game.AddToPot(CurrentBet);
        }

        public void Raise(int amount)
        {
            // Display number input
        }

        public void AllIn()
        {
            CurrentBet = CurrentMoney;
            Status = Status.AllIn;
        }
        // END OF PLAYER LOGIC

        // DRAWING
        public void Draw(int x, int y, bool reversed = false, bool hideCards = true)
        {
            string infoString = Format.PlayerInfo(Name, CurrentMoney);
            string blindString = HasSmallBlind
                               ? "(b)"
                               : HasBigBlind
                               ? "(B)"
                               : IsCurrentPlayer && reversed
                               ? "^"
                               : IsCurrentPlayer
                               ? "v"
                               : "   ";

            string statusString = Status == Status.AllIn
                                ? "ALL-IN " + Format.Currency(CurrentBet)
                                : Status == Status.Bet
                                ? "BET " + Format.Currency(CurrentBet)
                                : Status == Status.Called
                                ? "CALLED " + Format.Currency(CurrentBet)
                                : Status == Status.Checked
                                ? "CHECKED"
                                : Status == Status.Folded
                                ? "FOLDED" : Status == Status.Raised
                                ? "RAISED " + Format.Currency(CurrentBet)
                                : "";

            int blindWidth = blindString.Length;
            int infoWidth = infoString.Length;
            int statusWidth = statusString.Length;
            int cardsWidth = 8;

            int playerInfoHeight = 6;

            int maxWidth = Math.Max(Math.Max(cardsWidth, infoWidth), statusWidth);

            int yOffset = reversed ? playerInfoHeight : -playerInfoHeight;
            int drawDirection = reversed ? -1 : 1;

            DrawCenteredString(blindString, x, y + yOffset, IsCurrentPlayer ? ConsoleColor.Green : ConsoleColor.Gray);
            yOffset += drawDirection;
            DrawCenteredString(infoString, x, y + yOffset);
            yOffset += drawDirection;
            HoleCards.Draw(x, y + yOffset, hideCards, Status == Status.Folded, reversed);
            yOffset += drawDirection * 4;
            DrawCenteredString(statusString, x, y + yOffset);
            yOffset += drawDirection;

        }

        private void DrawCenteredString(string s, int x, int y, ConsoleColor c = ConsoleColor.Gray)
        {
            DrawEmptyCenteredString(x, y, 20);
            Console.SetCursorPosition(x - s.Length / 2, y);
            Console.ForegroundColor = c;
            Console.Write(s);
            Console.ResetColor();
        }

        private void DrawEmptyCenteredString(int x, int y, int n)
        {
            Console.SetCursorPosition(x - n / 2, y);
            for (int i = 0; i < n; i++)
            {
                Console.Write(" ");
            }
        }
        // END OF DRAWING

        // RESET
        public void Reset()
        {
            HoleCards.Reset();
        }
        // END OF RESET
    }
}