using BillJazzly.Bill;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BillJazzly.SingleTon
{
    public class DataHolder
    {
        private static DataHolder _Instance = null;
        private static object _pathlock = new object();
        private Dictionary<string, string> _Accounts = null;
        private Dictionary<int, Dictionary<string, List<JBill>>> _Bills = null;
        private string location = "Bin";
        private string account_file = "Data1.bill";
        private string bill_file = "Data2.bill";
        private DataHolder()
        {
            _Accounts = new Dictionary<string, string>();
            _Bills = new Dictionary<int, Dictionary<string, List<JBill>>>();
            LoadBills();
            InitBills();
        }
        private void InitBills()
        {
            if (_Bills.Count == 0)
            {
                List<JBill> list1 = new List<JBill>();
                DateTime date = new DateTime(2015, 02, 07, 16, 30, 00);
                list1.Add(new JBill.Builder().Name("Johnwesley").Price(12).Date(date).Description("Example").Build());

                Dictionary<string, List<JBill>> months1 = new Dictionary<string, List<JBill>>();
                months1.Add("Februari", list1);

                Dictionary<int, Dictionary<string, List<JBill>>> years = new Dictionary<int, Dictionary<string, List<JBill>>>();
                years.Add(2015, months1);
                _Bills = years;

                SaveBills();
            }
        }
        public static DataHolder Get
        {
            get
            {
                lock (_pathlock)
                {
                    if (_Instance == null)
                    { _Instance = new DataHolder(); }
                    return _Instance;
                }
            }
        }
        #region Accounts
        public Dictionary<string, string> Accounts
        {
            get { return _Accounts; }
        }
        public bool Contains(string username)
        { return _Accounts.ContainsKey(username); }
        public void SaveAccounts()
        {
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            using (StreamWriter writer = new StreamWriter(location + "/" + account_file))
            {
                foreach (string username in _Accounts.Keys)
                {
                    writer.WriteLine(EnCode(username + " - " + _Accounts[username]));
                }
            }
        }
        public void LoadAccounts()
        {
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            if (!File.Exists(location + "/" + account_file))
            { using (StreamWriter writer = new StreamWriter(location + "/" + account_file)) { writer.WriteLine(EnCode("admin") + " - " + EnCode("admin")); } }
            using (StreamReader reader = new StreamReader(location + "/" + account_file))
            {
                int index = 0;
                while (reader.Peek() >= 0)
                {
                    string line = DeCode(reader.ReadLine());
                    try
                    {
                        string[] info = line.Split('-');
                        _Accounts.Add(info[0].Trim(), info[1].Trim());
                    }
                    catch (FormatException) { Log("Format Exceptoin on line [" + index + "] + with data [{" + line + "}]"); }
                    index++;
                }
            }
        }
        #endregion
        #region Bills
        public Dictionary<int, Dictionary<string, List<JBill>>> Bills
        { get { return _Bills; } }
        public JBill GetBill(int year, string month, int index)
        { return _Bills[year][month][index]; }
        public List<JBill> GetBillsFromMonth(int year, string month)
        { return _Bills[year][month]; }
        public Dictionary<string, List<JBill>> GetMonthsFromYear(int year)
        { return _Bills[year]; }
        private void SaveBills()
        {
            Debug.WriteLine("Saving Bills");
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            using (StreamWriter writer = new StreamWriter(location + "/" + bill_file))
            {
                foreach (int year in _Bills.Keys)
                {
                    writer.WriteLine(EnCode("" + year));
                    Dictionary<string, List<JBill>> months = _Bills[year];
                    foreach (string month in months.Keys)
                    {
                        writer.WriteLine(EnCode(month));
                        List<JBill> bills = months[month];
                        foreach (JBill bill in bills)
                        {
                            string line = bill._Name + "-" + bill._Price + "-" + bill._Date + "-" + bill._Description;
                            writer.WriteLine(EnCode(line));
                        }
                        writer.WriteLine(EnCode("#EndMonth")); // TODO: Magic cookie
                    }
                    writer.WriteLine(EnCode("#EndYear")); // TODO: Magic cookie
                }
                writer.WriteLine(EnCode("#EndBills")); // TODO: Magic cookie
            }
        }
        private void LoadBills()
        {
            Debug.WriteLine("Loading Bills");
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            if (File.Exists(location + "/" + bill_file))
            {
                int readingState = 1;
                int year = -1;
                string month = null;
                bool billLooking = true;
                Dictionary<int, Dictionary<string, List<JBill>>> yearData = new Dictionary<int, Dictionary<string, List<JBill>>>();
                Dictionary<string, List<JBill>> monthData = new Dictionary<string, List<JBill>>();
                using (StreamReader reader = new StreamReader(location + "/" + bill_file))
                {
                    List<JBill> bills = new List<JBill>();
                    while (reader.Peek() >= 0)
                    {
                        string line = DeCode(reader.ReadLine()).Trim();
                        if (line.Equals("#EndBills")) { break; } // TODO: Magic cookie
                        else if (line.Equals("#EndYear")) // TODO: Magic cookie
                        {
                            readingState = 1;
                            if (yearData.ContainsKey(year))
                            { throw new NotSupportedException(); }
                            yearData.Add(year, monthData);
                            monthData = new Dictionary<string, List<JBill>>();
                            year = -1;
                        }
                        else if (line.Equals("#EndMonth")) // TODO: Magic cookie
                        {
                            readingState = 2;
                            if (monthData.ContainsKey(month))
                            { throw new NotSupportedException(); }
                            monthData.Add(month, bills);

                            month = null;
                        }
                        else
                        {
                            if (readingState == 1)
                            { year = int.Parse(line); readingState = 2; }
                            else if (readingState == 2)
                            { month = line; readingState = 3; }
                            else
                            {
                                string[] array = line.Split('-', ' ', ':');
                                DateTime date = new DateTime(int.Parse(array[4]), int.Parse(array[3]), int.Parse(array[2]), int.Parse(array[5]), int.Parse(array[6]), int.Parse(array[7]));
                                double price = double.Parse(array[1]);
                                JBill bill = new JBill.Builder().Name(array[0]).Price(price).Date(date).Description(array[array.Length - 1]).Build();
                                bills.Add(bill);
                            }
                        }
                    }
                }
                _Bills = yearData;
                foreach (int y in _Bills.Keys)
                    foreach (string m in _Bills[y].Keys)
                        foreach (JBill bill in _Bills[y][m])
                            Debug.WriteLine("Loaded Bill: " + bill.ToString());
            }
            else { MessageBox.Show("Could not load Old Bills. Something went terrible wrong!"); } // TODO: Magic cookie
        }
        #endregion
        public static string EnCode(string Text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Text);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string DeCode(string Text)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(Text);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static void Log(string line)
        { Debug.WriteLine("Log: " + Time() + " " + line); }
        public static string Time()
        { return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); }
    }
}
