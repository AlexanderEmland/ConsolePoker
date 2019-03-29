using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public static class Format
    {
        public static string Currency(int money)
        {
            string moneyString = "$";

            if (money >= 1000000)
            {
                moneyString += money / 1000000 + "M";
            }
            else if (money >= 10000)
            {
                moneyString += money / 1000 + "K";
            }
            else
            {
                moneyString += money;
            }

            return moneyString;
        }

        public static string PlayerInfo(string name, string formattedCurreny)
        {
            return name + " - " + formattedCurreny;
        }

        public static string PlayerInfo(string name, int money)
        {
            return PlayerInfo(name, Currency(money));
        }
    }
}
