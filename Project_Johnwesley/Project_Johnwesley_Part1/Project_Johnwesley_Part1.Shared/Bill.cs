using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Johnwesley_Part1
{
    public class Bill
    {
        public string payer { get; private set; }
        public double price { get; private set; }
        public DateTime time { get; private set; }
        public string description { get; private set; }
        private Bill(BillBuilder builder)
        {
            payer = builder.Payer();
            price = builder.Price();
            time = builder.Time();
            description = builder.Description();
        }
        public class BillBuilder
        {
            private string payer;
            private double price;
            private DateTime time;
            private string description;
            public BillBuilder()
            {
                this.payer = "Unknown";
                this.price = 0.0;
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string[] array = date.Split('-');
                this.time = new DateTime(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()));
                this.description = "None";
            }
            public string Payer() { return payer; }
            public BillBuilder Payer(string name) { this.payer = name; return this; }
            public double Price() { return price; }
            public BillBuilder Price(double price) { this.price = price; return this; }
            public DateTime Time() { return time; }
            public BillBuilder Time(DateTime time) { this.time = time; return this; }
            public string Description() { return description; }
            public BillBuilder Description(string description) { this.description = description; return this; }
            public Bill Build()
            { return new Bill(this); }
        }
        public override string ToString()
        { return payer + " - " + price; }
    }
}
