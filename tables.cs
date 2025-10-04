using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAA_1
{
    internal class tables
    {
        string[,] table;
        string name;
        int[] indexed;
        public tables(string tablename, string[] tableparam, int[] ifindexed) 
        {
            table = new string[tableparam.Length, 1];
            for (int i = 0; i < tableparam.Length; i++)
            {
                table[i, 0] = tableparam[i];
            }
            name = tablename;
            indexed = ifindexed;
        }

    }
}
