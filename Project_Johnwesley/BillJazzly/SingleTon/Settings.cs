using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillJazzly.SingleTon
{
    public class Settings
    {
        private static Settings _Instance = null;
        private static object _pathlock = new object();
        private Settings() { }
        public static Settings Get
        {
            get
            {
                lock (_pathlock)
                {
                    if (_Instance == null)
                    { _Instance = new Settings(); }
                    return _Instance;
                }
            }
        }
        private void LoadSettings()
        {
            // TODO: Method Can never crash!
        }
        private void SaveSettings()
        {
            // TODO: Method Can never crash!
        }
    }
}
