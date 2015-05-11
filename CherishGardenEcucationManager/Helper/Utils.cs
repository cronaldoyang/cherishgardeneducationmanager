using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherishGardenEducationManager.Helper
{
    class Utils
    {
        public static string[] splitStringWithReturn(string value)
        {
            //C#中回车是\r\n,先替换\r\n为|，再split
            return  value.Replace("\n", "|").Split('|');
        }
    }
}
