using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Logging
{

    //singleton //
    public class Logger
    {
        private static Logger instance;
        private static readonly object locker = new object();

        private Logger()
        {
            Console.WriteLine("Logger initialized");
        }

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new Logger();
                    }
                }
            }

            return instance;
        }

        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] {message}");
        }
    }
}