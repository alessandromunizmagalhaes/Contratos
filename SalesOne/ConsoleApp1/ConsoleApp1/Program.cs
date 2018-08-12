using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string cc = "4556364607935616";
            string res = "";
            if(cc.Length > 4)
            {
                int qty_of_sharps = cc.Length - 4;
                int pos_start_sufix = cc.Length - 4;
                string sufix = cc.Substring(pos_start_sufix, 4);
                res = res.PadLeft(qty_of_sharps, '#') + sufix;
            }
            else
            {
                res = cc;
            }

            Console.WriteLine(res);
            Console.ReadKey();
        }
    }
}
