using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillJazzly.SingleTon
{
    public class Short_Keys
    {
        public sealed class MessageBox
        {
            public const string CouldNotLoadBillFile = "Could not load Old Bills. Something went terrible wrong!";
            public const string BillNotFound = "Remove Bill Info Argument Unknown";
        }
        public sealed class Settings
        {
            public const string OverView_FontSize = "Overview Font Size";
            public const string Salary = "Default Salary";
            public const string Min_Bill_Year = "Min_Bill_Year";
            public const string Max_Bill_Year = "Max_Bill_Year";
            public const string Volume = "Volume";
            public const string Audio = "Audio";
            public const string Video = "Video";
            public const string Saved = "Settings has been saved";
            public const string Loaded = "Settings has been loaded";
            public const string On = "ON";
            public const string Off = "OFF";
        }
        public sealed class Invalid
        {
            public const string Year = "Invalid year value";
            public const string Salary = "Invalid salaray value";
            public const string FontSze = "Invalid font size value";
        }
        public sealed class Execute
        {
            public static string WrongInput(string feedback) { return "Wrong Input (" + feedback + ")"; }
        }
        public sealed class Feedback
        {
            public const string ToHigh = "To Low";
            public const string ToLow = "To High";
            public const string LettersNotNumbers = "Use Letters, not numbers";
            public const string WrongInputFormat = "Wrong Input, Format Error";
            public const string WrongInputDoesExists = "Wrong Input, Allready Exists";
            public const string SelectYear = "Select a year to continue";
            public const string SelectMonth = "Select a month to continue";
        }
        public sealed class Exception
        {
            public const string FormatFontSize = "Font size Format Error";
        }
        public sealed class ReadWriter
        {
            public const string EndMonth = "#EndMonth";
            public const string EndYear = "#EndYear";
            public const string EndBills = "#EndBills";
        }
        public sealed class Separate
        {
            public const char SplitSign = '&';
        }
    }
}
