using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Johnwesley_Part1
{
    public class Settings
    {
        private static Settings _Instance = null;
        private static object pathlock = new object();
        private string _AboutText = null;
        private Settings() { }
        public static Settings Get
        {
            get
            {
                if (_Instance == null)
                { _Instance = new Settings(); }
                return _Instance;
            }
        }
        public string AboutText
        {
            get
            {
                if (_AboutText == null)
                { CreateAboutText(); }
                return _AboutText;
            }
        }


        #region Create
        private void CreateAboutText()
        {
            string text = "";
            text += "This application is made by Roel Suntjens." + "\n";
            text += "The application was designed and for Johnwesley van der Wiel." + "\n";
            _AboutText = text;
        }
        public static string CreateKey(string year, string month)
        { return year + "_" + month + "Bill_Value_"; }
        public static string CreateYearKey()
        { return "Application_YearKey"; }
        public static string CreateMonthKey(string year)
        { return year + "_MonthKey"; }
        #endregion
        #region Convert Key
        public static string KeyToTotalMoneyKey(string originalKey)
        { return originalKey + "TotalMoney"; }
        public static string KeyToAmountOfBillsKey(string originalKey)
        { return originalKey + "Amount"; }
        #endregion
    }
}
