using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillJazzly.Bill
{
    public class RMSBill
    {
        public string _Name { get; private set; }
        public double _Price { get; private set; }
        public DateTime _Date { get; private set; }
        public TimeSpan _Time { get; private set; }
        public string _Description { get; private set; }
        public RMSBill(Builder builder)
        {
            this._Name = builder._Name;
            this._Price = builder._Price;
            this._Date = builder._Date;
            this._Description = builder._Description;
        }
        public string ButtonName()
        { return ButtonName(_Name, _Price.ToString()); }
        public static string ButtonName(string name, string price)
        { return name + " - " + price; }
        private string ToTwoChars(int value)
        { return (value < 10 ? "0" + value : "" + value); }
        public string DateTimeToString()
        { return _Date.Year + "-" + ToTwoChars(_Date.Month) + "-" + ToTwoChars(_Date.Day) + " " + ToTwoChars(_Date.Hour) + ":" + ToTwoChars(_Date.Minute) + ":" + ToTwoChars(_Date.Second); }
        public override string ToString()
        { return "Name[" + _Name + "], Price[" + _Price + "], " + DateTimeToString() + ", Description[" + _Description + "]"; }
        public class Builder
        {
            public Builder()
            {
                _Name = "Unknown";
                _Price = 0.0;
                _Date = new DateTime(2000, 01, 01, 12, 00, 00);
                _Description = "Unknown";
            }
            public string _Name { get; private set; }
            public double _Price { get; private set; }
            public DateTime _Date { get; private set; }
            public string _Description { get; private set; }
            public Builder Name(string name) { _Name = name; return this; }
            public Builder Price(double price) { _Price = price; return this; }
            public Builder Date(DateTime date) { _Date = date; return this; }
            public Builder Description(string description) { _Description = description; return this; }
            public RMSBill Build()
            { return new RMSBill(this); }
        }
    }
}
