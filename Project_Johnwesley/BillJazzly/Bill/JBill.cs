using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillJazzly.Bill
{
    public class JBill
    {
        public string _Name { get; private set; }
        public double _Price { get; private set; }
        public DateTime _Date { get; private set; }
        public TimeSpan _Time { get; private set; }
        public string _Description { get; private set; }
        public JBill(Builder builder)
        {
            this._Name = builder._Name;
            this._Price = builder._Price;
            this._Date = builder._Date;
            this._Description = builder._Description;
        }
        public string ButtonName()
        { return _Name + " - " + _Price; }
        public string DateTimeToString()
        { return _Date.Year + "-" + _Date.Month + "-" + _Date.Day + " " + _Date.Hour + ":" + _Date.Minute + ":" + _Date.Second; }
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
            public JBill Build()
            { return new JBill(this); }
        }
    }
}
