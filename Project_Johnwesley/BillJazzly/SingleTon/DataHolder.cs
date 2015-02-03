using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillJazzly.SingleTon
{
    public class DataHolder
    {
        private static DataHolder _Instance = null;
        private static object _pathlock = new object();
        private Dictionary<string, string> _Accounts = null;
        private string location = "Bin";
        private string file = "Data.bill";
        private DataHolder()
        { }
        public DataHolder Get
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
        public Dictionary<string, string> Accounts
        {
            get { return _Accounts; }
        }
        public bool Contains(string username)
        { return _Accounts.ContainsKey(username); }
        public void Save()
        {
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
        }
        public void Load()
        {
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            if (!File.Exists(location + "/" + file))
            { using (StreamWriter writer = new StreamWriter(location + "/" + file)) { writer.WriteLine(EnCode("admin") + " - " + EnCode("admin")); } }
            using (StreamReader reader = new StreamReader(location + "/" + file))
            {
                int index = 0;
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    try
                    {
                        string[] info = line.Split('-');
                        _Accounts.Add(DeCode(info[0].Trim()), DeCode(info[1].Trim()));
                    }
                    catch (FormatException) { Log("Format Exceptoin on line [" + index + "] + with data [{" + line + "}]"); }
                    index++;
                }
            }
        }
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
