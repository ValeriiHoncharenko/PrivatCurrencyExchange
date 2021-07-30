using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Telegram.Bot;
using System;
using Telegram.Bot.Args;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;

namespace PrivatCurrencyExchange
{
    class CurrencyRate
    {
        public static void OnGet(Enum currency, DateTime date)
        {
            using (var WebClient = new WebClient())
            {
                string pathJson = "https://api.privatbank.ua/p24api/exchange_rates?json&date=" + date.ToString("d", CultureInfo.CreateSpecificCulture("ru-RU"));
                string jsonString = WebClient.DownloadString(pathJson);
                RootObject rootobject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                List<ExchangeRate> exchangeRates = (from n in rootobject.exchangeRate
                                                    select n).ToList();
                foreach (var n in from n in exchangeRates
                                  where n.currency == currency.ToString()
                                  select n)
                {
                    Program.Purshase = (double)n.purchaseRate;
                    Program.Sale = (double)n.saleRate;
                }
            }
        }
    }
}
