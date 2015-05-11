using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout_Kata
{
    [TestClass]
    public class CheckoutTests
    {
        [TestMethod]
        public void ScanASingleItemThroughCheckout()
        {
            var item = new Item ("Tomato Soup", 0.60m);

            var checkout = new Checkout();

            checkout.Scan(item);

            var total = checkout.GetRunningTotal();

            Assert.AreEqual(.60m, total);
        }

        [TestMethod]
        public void ScanMultipleItemsThroughCheckout()
        {
            var checkout = new Checkout();

            checkout.Scan(new Item("Tomato Soup", 0.60m));
            checkout.Scan(new Item("Toothbrush", 1.20m));

            var total = checkout.GetRunningTotal();

            Assert.AreEqual(1.80m, total);
        }

        [TestMethod]
        public void ScanASinglePerishedItemThroughCheckout()
        {
            var checkout = new Checkout();

            checkout.Scan(new PerishableItem("Apples", 1.00m, new DateTime(2015,4,1)));

            var total = checkout.GetRunningTotal();

            Assert.AreEqual(.90m, total);
        }

        [TestMethod]
        public void ScanMultipleMixedItemsThroughCheckout()
        {
            var checkout = new Checkout();

            checkout.Scan(new PerishableItem("Apples", 1.00m, new DateTime(2015, 4, 1)));
            checkout.Scan(new PerishableItem("Bananas", 1.00m, new DateTime(2015, 6, 1)));
            checkout.Scan(new Item("Toothbrush", 1.20m));

            var total = checkout.GetRunningTotal();

            Assert.AreEqual(3.10m, total);
        }
    }

    public class Checkout
    {
        private readonly List<ItemBase> _items = new List<ItemBase>();

        public void Scan(ItemBase item)
        {
            _items.Add(item);
        }

        public decimal GetRunningTotal()
        {
            return _items.Sum(x => x.GetPrice());
        }
    }

    public class PerishableItem : ItemBase
    {
        private readonly DateTime _useBy;

        public PerishableItem(string description, decimal price, DateTime useBy)
        {
            _useBy = useBy;
            Description = description;
            Price = price;
        }

        public override decimal GetPrice()
        {
            return _useBy.CompareTo(DateTime.Now) < 0 ? Price = Price - Price * 0.1m : Price;
        }
    }

    public class Item : ItemBase
    {
        
        public Item(string description, decimal price)
        {
            Description = description;
            Price = price;
        }

        public override decimal GetPrice()
        {
            return Price;
        }
    }

    public abstract class ItemBase
    {
        public string Description { get; set; }
        public decimal Price { get; set; }

        public abstract decimal GetPrice();
    }
}
