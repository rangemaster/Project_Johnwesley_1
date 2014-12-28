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
        #endregion
    }
}
