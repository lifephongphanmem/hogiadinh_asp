using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLNCC.Helper
{
    public class Pagination
    {
        public static List<int> GetRangePage(int pageTotal, int pageCurrent)
        {
            int start = 0;
            int stop = 0;
            if (pageTotal <= 5)
            {
                start = 1;
                stop = pageTotal;
            }
            else
            {
                if (pageCurrent <= 3)
                {
                    start = 1;
                    stop = 5;
                }
                else if (pageCurrent + 1 >= pageTotal)
                {
                    start = pageTotal - 4;
                    stop = pageTotal;
                }
                else
                {
                    start = pageCurrent - 2;
                    stop = pageCurrent + 2;
                }
            }
            List<int> intRange = new List<int>();
            for (int i = start; i <= stop; i++)
            {
                intRange.Add(i);
            }

            return intRange;
        }
    }
}
