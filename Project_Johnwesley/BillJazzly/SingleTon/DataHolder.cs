using BillJazzly.Bill;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace BillJazzly.SingleTon
{
    public class DataHolder
    {
        private static DataHolder _Instance = null;
        private static object _pathlock = new object();
        private Dictionary<string, string> _Accounts = null;
        private Dictionary<string, int> _Settings = null;
        private Dictionary<int, Dictionary<string, Tuple<double, List<RMSBill>>>> _Bills = null;
        private string location = "Bin";
        private string account_file = "Data1.bill";
        private string bill_file = "Data2.bill";
        private string settings_file = "Data3.bill";
        private DataHolder()
        {
            _Accounts = new Dictionary<string, string>();
            _Settings = new Dictionary<string, int>();
            _Bills = new Dictionary<int, Dictionary<string, Tuple<double, List<RMSBill>>>>();
            LoadBills();
            LoadSettings();
            if (_Bills.Count == 0)
                InitBills();
        }
        private void InitBills()
        {
            List<RMSBill> list1 = new List<RMSBill>();
            DateTime date = new DateTime(2015, 02, 07, 16, 30, 00);
            list1.Add(new RMSBill.Builder().Name("Roel").Description("Here is a long description to test the textwrapping. And to make it interesting it gets even longer. It has to be very long...... longer...... llonger..... asdfa;dbjpaosepowuehpasd;bajkn;sfahpsuhpwauhe veryyyyy long...... even longer").Build());
            list1.Add(new RMSBill.Builder().Name("Johnwesley").Price(12).Date(date).Description("Example").Build());
            list1.Add(new RMSBill.Builder().Name("Ashley").Price(13).Description("An other Example").Build());
            Tuple<double, List<RMSBill>> list = new Tuple<double, List<RMSBill>>(2000, list1);
            Dictionary<string, Tuple<double, List<RMSBill>>> months1 = new Dictionary<string, Tuple<double, List<RMSBill>>>();
            months1.Add("Februari", list);

            Dictionary<int, Dictionary<string, Tuple<double, List<RMSBill>>>> years = new Dictionary<int, Dictionary<string, Tuple<double, List<RMSBill>>>>();
            years.Add(2015, months1);
            _Bills = years;

            SaveBills();
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

        #region Settings
        public void SaveSettings()
        {
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            using (StreamWriter writer = new StreamWriter(location + "/" + settings_file))
            {
                foreach (string key in _Settings.Keys)
                { writer.WriteLine(EnCode(key + " & " + _Settings[key])); }
            }
        }
        public void LoadSettings()
        {
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            if (File.Exists(location + "/" + settings_file))
            {
                _Settings.Clear();
                using (StreamReader reader = new StreamReader(location + "/" + settings_file))
                {
                    while (reader.Peek() >= 0)
                    {
                        string line = DeCode(reader.ReadLine());
                        string[] array = line.Split('&');
                        _Settings.Add(array[0].Trim(), int.Parse(array[1].Trim()));
                    }
                }
            }
            else
            {
                DefaultSettings();
            }
        }
        public void DefaultSettings()
        {
            Debug.WriteLine("Default Settings");
            SetSetting(Short_Keys.Settings.OverView_FontSize, 12);
            SetSetting(Short_Keys.Settings.Salary, 0);
            SetSetting(Short_Keys.Settings.Min_Bill_Year, 1900);
            SetSetting(Short_Keys.Settings.Max_Bill_Year, 2100);
            SetSetting(Short_Keys.Settings.Volume, 3);
            SetSetting(Short_Keys.Settings.Audio, 0);
            SetSetting(Short_Keys.Settings.Video, 0);
            SaveSettings();
        }
        public void SetSetting(string setting_name, int setting_value)
        {
            try { _Settings[setting_name] = setting_value; }
            catch (ArgumentException) { throw new ArgumentException(); }
        }
        public int GetSettingsValue(string setting_name)
        { Debug.WriteLine("GetSettingsValue(" + setting_name + ") = " + _Settings[setting_name] + ")"); return _Settings[setting_name]; }
        #endregion

        #region Bills
        public Dictionary<int, Dictionary<string, Tuple<double, List<RMSBill>>>> Bills
        { get { return _Bills; } }
        public RMSBill GetBill(int year, string month, int index)
        { return _Bills[year][month].Item2[index]; }
        public Tuple<double, List<RMSBill>> GetBillsFromMonth(int year, string month)
        { return _Bills[year][month]; }
        public void SetSalaryOfMonth(int year, string month, double salary)
        { _Bills[year][month] = new Tuple<double, List<RMSBill>>(salary, _Bills[year][month].Item2); }
        public double GetSalaryOfMonth(int year, string month)
        { return _Bills[year][month].Item1; }
        public Dictionary<string, Tuple<double, List<RMSBill>>> GetMonthsFromYear(int year)
        { return _Bills[year]; }
        public void SortBillsOnYears()
        {
            List<Tuple<int, Dictionary<string, Tuple<double, List<RMSBill>>>>> Bill_List = new List<Tuple<int, Dictionary<string, Tuple<double, List<RMSBill>>>>>();
            foreach (int key in _Bills.Keys)
            { Bill_List.Add(new Tuple<int, Dictionary<string, Tuple<double, List<RMSBill>>>>(key, _Bills[key])); }
            int amount = 1;
            while (amount > 0)
            {
                amount = 0;
                for (int lower = 1; lower < Bill_List.Count; lower++)
                {
                    if (Bill_List[lower - 1].Item1 < Bill_List[lower].Item1)
                    {
                        var x = Bill_List[lower - 1];
                        Bill_List[lower - 1] = Bill_List[lower];
                        Bill_List[lower] = x;
                        amount++;
                    }
                }
            }
            _Bills.Clear();
            for (int i = 0; i < Bill_List.Count; i++)
            { _Bills.Add(Bill_List[i].Item1, Bill_List[i].Item2); }
            Debug.WriteLine("Sort Bill On Years");
        }
        public void SaveBills()
        {
            Debug.WriteLine("Saving Bills");
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            using (StreamWriter writer = new StreamWriter(location + "/" + bill_file))
            {
                foreach (int year in _Bills.Keys)
                {
                    writer.WriteLine(EnCode("" + year));
                    Dictionary<string, Tuple<double, List<RMSBill>>> months = _Bills[year];
                    foreach (string month in months.Keys)
                    {
                        writer.WriteLine(EnCode(month + " " + Short_Keys.Separate.SplitSign + " " + months[month].Item1.ToString()));
                        List<RMSBill> bills = months[month].Item2;
                        foreach (RMSBill bill in bills)
                        {
                            string line = bill._Name + "-" + bill._Price + "-" + bill._Date + "-" + bill._Description;
                            writer.WriteLine(EnCode(line));
                        }
                        writer.WriteLine(EnCode(Short_Keys.ReadWriter.EndMonth));
                    }
                    writer.WriteLine(EnCode(Short_Keys.ReadWriter.EndYear));
                }
                writer.WriteLine(EnCode(Short_Keys.ReadWriter.EndBills));
            }
        }
        public void LoadBills()
        {
            Debug.WriteLine("Loading Bills");
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            if (File.Exists(location + "/" + bill_file))
            {
                int readingState = 1;
                int year = -1;
                string month = null;
                Dictionary<int, Dictionary<string, Tuple<double, List<RMSBill>>>> yearData = new Dictionary<int, Dictionary<string, Tuple<double, List<RMSBill>>>>();
                Dictionary<string, Tuple<double, List<RMSBill>>> monthData = new Dictionary<string, Tuple<double, List<RMSBill>>>();
                using (StreamReader reader = new StreamReader(location + "/" + bill_file))
                {
                    List<RMSBill> bills = new List<RMSBill>();
                    double salary = -1;
                    if (_Settings.Count > 0)
                        salary = GetSettingsValue(Short_Keys.Settings.Salary);
                    while (reader.Peek() >= 0)
                    {
                        string line = DeCode(reader.ReadLine()).Trim();
                        if (line.Equals(Short_Keys.ReadWriter.EndBills)) { break; }
                        else if (line.Equals(Short_Keys.ReadWriter.EndYear))
                        {
                            readingState = 1;
                            if (yearData.ContainsKey(year))
                            { throw new NotSupportedException(); }
                            yearData.Add(year, monthData);
                            monthData = new Dictionary<string, Tuple<double, List<RMSBill>>>();
                            year = -1;
                        }
                        else if (line.Equals(Short_Keys.ReadWriter.EndMonth))
                        {
                            readingState = 2;
                            if (monthData.ContainsKey(month))
                            { throw new NotSupportedException(); }
                            monthData.Add(month, new Tuple<double, List<RMSBill>>(salary, bills));
                            bills = new List<RMSBill>();
                            month = null;
                        }
                        else
                        {
                            if (readingState == 1)
                            { year = int.Parse(line); readingState = 2; }
                            else if (readingState == 2)
                            {
                                string[] array = line.Split(Short_Keys.Separate.SplitSign);
                                month = array[0].Trim();
                                salary = double.Parse(array[1].Trim());
                                readingState = 3;
                            }
                            else
                            {
                                string[] array = line.Split('-', ' ', ':');
                                string description = " ";
                                for (int i = 8; i < array.Length; i++)
                                { description += array[i] + " "; }
                                DateTime date = new DateTime(int.Parse(array[4]), int.Parse(array[3]), int.Parse(array[2]), int.Parse(array[5]), int.Parse(array[6]), int.Parse(array[7]));
                                double price = double.Parse(array[1]);
                                RMSBill bill = new RMSBill.Builder().Name(array[0]).Price(price).Date(date).Description(description.Trim()).Build();
                                bills.Add(bill);
                            }
                        }
                    }
                }
                _Bills = yearData;
            }
            else { MessageBox.Show(Short_Keys.MessageBox.CouldNotLoadBillFile); }
        }
        #endregion

        #region Encoding / Decoding
        public static string EnCode(string Text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Text);
            return System.Convert.ToBase64String(plainTextBytes);
            // return Text;
        }
        public static string DeCode(string Text)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(Text);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            // return Text;
        }
        #endregion
        public static void Log(string line)
        { Debug.WriteLine("Log: " + Time() + " " + line); }
        public static string Time()
        { return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); }
    }
}
