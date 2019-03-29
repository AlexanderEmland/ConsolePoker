using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLibrary.Console;
using MyLibrary.Console.Drawing;
using MyLibrary.Console.Utils;

namespace Poker
{
    class Game
    {
        // DRAWING CONSTANTS (refactor to drawing class?)
        private const int cardMargin = 5;
        private const int playerMargin = 0;
        private const int cardInnerWidth = 14; //12
        private const int cardInnerHeight = 11; //10
        private const int communityWidth = 5 * (2 + cardInnerWidth) + 4 * cardMargin;
        private const int communityHeight = cardInnerHeight + 2;
        

        private Dealer Dealer;
        private List<Player> Players;
        private List<Card> CommunityCards;
        private Random rng = new Random();

        // GAMEPLAY VARIABLES
        private int currentPlayerIndex = -1;
        private int smallBlindIndex = -1;
        private int bigBlindIndex = -1;
        private int smallBlindAmount = 1;
        private int bigBlindAmount = 2;
        private int maxBet = 0;
        private int pot = 0;
        private int opponentCount = 1;


        private static readonly string[] nameList =
        {
            "James",
            "John",
            "Robert",
            "Michael",
            "William",
            "David",
            "Richard",
            "Charles",
            "Joseph",
            "Thomas",
            "Christopher",
            "Daniel",
            "Paul",
            "Mark",
            "Donald",
            "George",
            "Kenneth",
            "Steven",
            "Edward",
            "Brian",
            "Ronald",
            "Anthony",
            "Kevin",
            "Jason",
            "Jeff",
            "Mary",
            "Patricia",
            "Linda",
            "Barbara",
            "Elizabeth",
            "Jennifer",
            "Maria",
            "Susan",
            "Margaret",
            "Dorothy",
            "Lisa",
            "Nancy",
            "Karen",
            "Betty",
            "Helen",
            "Sandra",
            "Donna",
            "Carol",
            "Ruth",
            "Sharon",
            "Michelle",
            "Laura",
            "Sarah",
            "Kimberly",
            "Deborah",
        };

        public Game(int opponents)
        {
            //SmallBlindIndex = -1;
            //BigBlindIndex = -1;
            opponentCount = opponents;
            Dealer = new Dealer();
            CommunityCards = new List<Card>();
            Players = new List<Player>() { new Player("Alex", this) };
            for (int i = 0; i < opponentCount; i++)
            {
                Players.Add(new Opponent(nameList[rng.Next(0, nameList.Length)], this));
            }
            Reset();
        }

        public void Reset()
        {
            Console.Clear();
            ResetParticipants();
            ResetCommunityCards();
            Play();
            //Display cards (hide opponents')

            //Bet

            //Flop
        }

        public void Play()
        {
            // Reset cards but keep money
            // Setup
            ResetAllCards();
            HandOutCards();
            DecideBlinds();
            DrawTable();
            DrawGameInfo();

            //while (Console.ReadKey().Key == ConsoleKey.Enter)
            //{
            //    GetGetCurrentPlayer().MakeDecision(maxBet);
            //    NextPlayer();
            //    DrawTable();
            //}
            // Betting rounds
            
            // Showdown
        }

        // DRAWING
        private void DrawCommunityCards()
        {
            int totalCommunityWidth = 5 * (2 + cardInnerWidth) + 4 * cardMargin;
            int sideMargin = (Program.SCREEN_WIDTH - totalCommunityWidth) / 2;
            for (int i = 4; i >= 0; i--)
            {
                Card c = CommunityCards.ElementAt(i);

                if (c.Suit == SuitType.None)
                    c.DrawBigHidden(sideMargin + (2 + cardInnerWidth + cardMargin) * i,
                            -0 + Program.SCREEN_HEIGHT / 2 - (2 + cardInnerHeight) / 2,
                            cardInnerWidth,
                            cardInnerHeight);
                else
                    c.DrawBig(sideMargin + (2 + cardInnerWidth + cardMargin) * i,
                            -0 + Program.SCREEN_HEIGHT / 2 - (2 + cardInnerHeight) / 2,
                            cardInnerWidth,
                            cardInnerHeight);
            }
        }

        private void DrawTable()
        {
            DrawCommunityCards();
            DrawUpperPlayers();
            DrawLowerPlayers();
        }

        private void DrawGameInfo()
        {
            Draw.TextAlign = TextAlign.Center;
            Draw.SideMargin = 0;

            TextBox textBox = new TextBox();
            textBox.AddString($"Pot: ${pot}");
            textBox.AddString($"Blinds: ${smallBlindAmount}/${bigBlindAmount}");
            textBox.Position = new Point(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT/2);
            textBox.VerticalAlignment = VerticalAlignment.Bottom;
            textBox.HorizontalAlignment = HorizontalAlignment.Center;

            Draw.String(Text.RepeatChar('─', Program.SCREEN_WIDTH - Draw.SideMargin * 2), new Point(0, Program.SCREEN_HEIGHT - 2));
            Draw.String("(C)heck", AnchorPoint.Bottom, -1);
            Draw.TextAlign = TextAlign.Left;
            Draw.String("(C)heck", new Point(2, Draw.SCREEN_HEIGHT-1));
            Draw.TextAlign = TextAlign.Right;
            Draw.String("(C)heck", new Point(Draw.SCREEN_WIDTH-2, Draw.SCREEN_HEIGHT - 1));

            //textBox.Move(new Point(Draw.SCREEN_WIDTH / 2, Draw.SCREEN_HEIGHT / 2 + 5));

            Draw.TextAlign = TextAlign.Right;
            Draw.TextBox(textBox);
            //Draw.String(textBox.Width.ToString(), AnchorPoint.Right);
            //Draw.String(textBox.Height.ToString(), AnchorPoint.Right, 1);
            //Draw.DebugTextAlign("Hello world!");
            //Draw.DebugTextAlign("hello");
        }

        private void DrawUpperPlayers()
        {
            // Upper players = second half of participants (majority)
            int firstPlayerIndex = Players.Count / 2;
            int playersToGet = Players.Count - firstPlayerIndex;
            int playerSpacing = ((Program.SCREEN_WIDTH) / (playersToGet + 1));

            var upperPlayers = Players.GetRange(firstPlayerIndex, playersToGet);

            foreach (var p in upperPlayers)
            {
                int playerPositionX = (upperPlayers.IndexOf(p) + 1) * playerSpacing;
                int playerPositionY = Program.SCREEN_HEIGHT / 2 - communityHeight / 2 - 1 - playerMargin;
                p.Draw(playerPositionX,
                       playerPositionY,
                       false,
                       p is Opponent);
            }
        }

        private void DrawLowerPlayers()
        {
            // Lower players = first half of participants (minority)
            int playersToGet = Players.Count / 2;
            int playerSpacing = ((Program.SCREEN_WIDTH) / (playersToGet + 1));

            var lowerPlayers = Players.GetRange(0, playersToGet);
            lowerPlayers.Reverse();

            foreach (var p in lowerPlayers)
            {
                int playerPositionX = (lowerPlayers.IndexOf(p) + 1) * playerSpacing;
                int playerPositionY = Program.SCREEN_HEIGHT / 2 + communityHeight / 2 + 1 + playerMargin;
                p.Draw(playerPositionX,
                       playerPositionY,
                       true,
                       p is Opponent);
                //Console.SetCursorPosition(playerPositionX, playerPositionY);
                //Console.Write(p is Opponent ? 'O' : 'P');
            }
        }
        // END OF DRAWING

        // GAME LOGIC
        public void AddToPot(int amount)
        {
            pot += amount;

        }

        private void DecideBlinds()
        {
            smallBlindIndex = smallBlindIndex < 0
                            ? rng.Next(Players.Count)
                            : (smallBlindIndex + 1) % Players.Count;

            bigBlindIndex = (smallBlindIndex + 1) % Players.Count;
            currentPlayerIndex = (bigBlindIndex + 1) % Players.Count;

            smallBlindAmount = 1;

            UpdatePlayerStates();
            PayBlinds();
        }

        private void NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
            UpdatePlayerStates();
        }

        private void UpdatePlayerStates()
        {
            foreach (var player in Players)
            {
                player.HasSmallBlind = Players.IndexOf(player) == smallBlindIndex;
                player.HasBigBlind = Players.IndexOf(player) == bigBlindIndex;
                player.IsCurrentPlayer = Players.IndexOf(player) == currentPlayerIndex;
            }
        }

        private void HandOutCards()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (Player p in Players)
                    Dealer.Deal(p);
            }
        }

        private void PayBlinds()
        {
            GetSmallBlindPlayer().Bet(smallBlindAmount);
            GetBigBlindPlayer().Bet(bigBlindAmount);
        }

        private void ExpandCommunity()
        {
            int cardsToAdd = 0;
            int emptyCards = CommunityCards.Count(c => c.Suit == SuitType.None);

            switch (emptyCards)
            {
                case 5:
                    cardsToAdd = 3;
                    break;
                case 2:
                case 1:
                    cardsToAdd = 1;
                    break;
                default:
                    return;
            }
            if (cardsToAdd <= emptyCards)
            {
                Dealer.Deal(CommunityCards, cardsToAdd);
            }
        }
        // END OF GAME LOGIC 

        // RESET
        private void ResetAllCards()
        {
            ResetDealer();
            ResetCommunityCards();
            ResetHoleCards();
        }

        private void ResetDealer()
        {
            Dealer.Reset();
        }

        private void ResetHoleCards()
        {
            foreach (Player p in Players)
            {
                p.HoleCards.Reset();
            }
        }

        private void ResetParticipants()
        {
            foreach (Player o in Players)
                o.Reset();
        }

        private void ResetCommunityCards()
        {
            CommunityCards.Clear();
            for (int i = 0; i < 5; i++)
            {
                CommunityCards.Add(new Card(0, SuitType.None));
            }
        }

        // END OF RESET

        // HELPER FUNCTIONS
        private Player GetGetCurrentPlayer()
        {
            return Players.ElementAt(currentPlayerIndex);
        }

        private Player GetSmallBlindPlayer()
        {
            return Players.ElementAt(smallBlindIndex);
        }

        private Player GetBigBlindPlayer()
        {
            return Players.ElementAt(bigBlindIndex);
        }
        // END OF HELPER FUNCTIONS
    }
}
