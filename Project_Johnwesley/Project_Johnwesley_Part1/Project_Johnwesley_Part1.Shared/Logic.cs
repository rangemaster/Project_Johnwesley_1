using Project_Johnwesley_Part1.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Project_Johnwesley_Part1
{
    public class Logic
    {
        public static int Method1 = 1;
        public static int Method2 = 2;
        private Dictionary<int, RMHandler> handlers;

        public Logic()
        { Init(); }
        private void Init()
        { Registration(); }
        private void Registration()
        {
            this.handlers = new Dictionary<int, RMHandler>();
            AddHandler(Method1, new Method1());
            AddHandler(Method2, new Method2());
        }
        public void AddHandler(int key, RMHandler handler)
        { this.handlers.Add(key, handler); }
        public void handle(int index)
        {
            if (handlers.ContainsKey(index))
            { handlers[index].handle(); }
            else
            { Debug.WriteLine("Handler: " + index + " , does not exists!"); }
        }
    }
}
