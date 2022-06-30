using System;
using static System.Console;

namespace TryEvents
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bitcoin = new Bitcoin(1000);
            var user1 = new User();
            user1.Buy(bitcoin); //event subscription

            WriteLine("Change price: ");
            string newprice = ReadLine();
            bitcoin.Price = int.Parse(newprice);

            user1.Sell(bitcoin);
            WriteLine("Change price: ");
            newprice = ReadLine();
            bitcoin.Price = int.Parse(newprice);
        }

        //Event Handler 
        public static void PriceChange(object sender, PriceEventArgs e)
        {
            WriteLine($"Price change: {e.NewPrice}");
        }

        public class PriceEventArgs : EventArgs
        {
            public int NewPrice { get; set; }
            public PriceEventArgs(int price)
            {
                NewPrice = price;
            }
        }

        public interface Crypto
        {
            public event EventHandler<PriceEventArgs> PriceChanged;
        }

        public class Bitcoin : Crypto //Publisher
        {
            public event EventHandler<PriceEventArgs> PriceChanged;
            private static int price;
            public int Price
            {
                get { return price; }
                set
                {
                    price = value;
                    OnPriceChange(new PriceEventArgs(value));
                }
            }
            public Bitcoin(int price)
            {
                Price = price;
            }
            protected void OnPriceChange(PriceEventArgs e)
            {
                PriceChanged?.Invoke(this, e); //invoke event
            }
        }
        public class User //Subscriber
        {
            public void Buy(Crypto crypto)
            {
                crypto.PriceChanged += PriceChange; //subscription
            }
            public void Sell(Crypto crypto)
            {
                crypto.PriceChanged -= PriceChange; //desubscription
            }
        }
    }
}
