using Telegram.Bot;
using System;
using Telegram.Bot.Args;
using Newtonsoft.Json;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static PrivatCurrencyExchange.Currency;
using System.Linq;

namespace PrivatCurrencyExchange
{
    class Program
    {
        private static TelegramBotClient client;
        public static double Purshase;
        public static double Sale;
        private static string date;
        private static int currencyFound = 0;
        static void Main()
        {            
            client = new TelegramBotClient(Config.Token);
            client.StartReceiving();
            client.OnMessage += OnMassageHandler;
            Console.WriteLine("[PrivatCurrencyExchange1_bot]: Bot started");
            Console.ReadKey();
            client.StopReceiving();
        }
        private static async void OnMassageHandler(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            string rezult = "";
            if (message.Text != null)
            {
                string a = e.Message.Text;
                string[] subs = a.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                Match match = Regex.Match(subs.LastOrDefault(), @"\d\d[.]\d\d[.]\d\d\d\d");
                if (match.Success)
                {
                    date = match.Captures[0].Value.ToString();
                    var usCulture = new System.Globalization.CultureInfo("ru-RU");
                    DateTime dt;                    
                    if (DateTime.TryParse(date, usCulture.DateTimeFormat, System.Globalization.DateTimeStyles.None, out dt))
                    {                       
                        foreach (Currencies nVal in Enum.GetValues(typeof(Currencies)))
                        {
                            if (subs[0].ToUpper() == (nVal.ToString()))
                            {                                
                                currencyFound = 1;
                                CurrencyRate.OnGet(nVal, dt);
                                rezult = "Exchange rates  " + nVal +": " + Purshase.ToString() + "/" + Sale.ToString() + " грн.";
                                break;
                            }                          
                        }
                        if(currencyFound == 0)
                        {                           
                            rezult = "Enter  the correct currency "; 
                        }
                    }
                    else
                    {                        
                        rezult = "Enter the correct DATE 'dd.mm.yyyy'";
                    }
                }
                else
                {
                    rezult = "Enter the correct DATE 'dd.mm.yyyy'";
                }
                     currencyFound = 0;               
                     await client.SendTextMessageAsync(message.Chat.Id, rezult, replyToMessageId: message.MessageId);
                
            } 
        }                
    }
}
