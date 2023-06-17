using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionStore.BLL
{
    public class CurrencyRate
    {
        Dictionary<string, decimal> _rate;

        public CurrencyRate()
        {
            _rate = new Dictionary<string, decimal>() 
            {
                { "RUBUSD", 1 },
                { "USDRUB", 2 },
                { "RUBEUR", 3 },
                { "EURRUB", 4 },
                { "RUBJPY", 5 },
                { "JPYRUB", 6 },
                { "RUBCNY", 7 },
                { "CNYRUB", 8 },
                { "RUBRSD", 9 },
                { "RSDRUB", 10 },
                { "RUBBGN", 11 },
                { "BGNRUB", 12 },
                { "RUBARS", 13 },
                { "ARSRUB", 14 },
                { "USDEUR", 15 },
                { "EURUSD", 16 },
                { "USDJPY", 17 },
                { "JPYUSD", 18 },
                { "USDCNY", 19 },
                { "CNYUSD", 20 },
                { "USDRSD", 21 },
                { "RSDUSD", 22 },
                { "USDBGN", 23 },
                { "BGNUSD", 24 },
                { "USDARS", 25 },
                { "ARSUSD", 26 },
                { "EURJPY", 27 },
                { "JPYEUR", 28 },
                { "EURCNY", 29 },
                { "CNYEUR", 30 },
                { "EURRSD", 31 },
                { "RSDEUR", 32 },
                { "EURBGN", 33 },
                { "BGNEUR", 34 },
                { "EURARS", 35 },
                { "ARSEUR", 36 },
                { "JPYCNY", 37 },
                { "CNYJPY", 38 },
                { "JPYRSD", 39 },
                { "RSDJPY", 40 },
                { "JPYBGN", 41 },
                { "BGNJPY", 42 },
                { "JPYARS", 43 },
                { "ARSJPY", 44 },
                { "CNYRSD", 45 },
                { "RSDCNY", 46 },
                { "CNYBGN", 47 },
                { "BGNCNY", 48 },
                { "CNYARS", 49 },
                { "ARSCNY", 50 },
                { "RSDBGN", 51 },
                { "BGNRSD", 52 },
                { "RSDARS", 53 },
                { "ARSRSD", 54 },
                { "BGNARS", 55 },
                { "ARSBGN", 56 }
            };

        }
        
        public decimal GetRate(string currency1, string currency2)
        {
            return _rate[currency1+currency2];
        }
    }
}
