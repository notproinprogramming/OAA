using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAA_1
{
    internal class tables
    {
        public string[,] table;
        public string name;
        //public bool[] indexed;
        public tables(string tablename, string[] tableparam/*, bool[] ifindexed*/) 
        {
            table = new string[tableparam.Length, 1];
            for (int i = 0; i < tableparam.Length; i++)
            {
                table[i, 0] = tableparam[i];
            }
            name = tablename;
            //indexed = ifindexed;
        }
        public void InsertRow(string[] values)
        {
            if (values.Length != table.GetLength(0))
                throw new ArgumentException("Забагато аргументів");
            int oldCols = table.GetLength(0);
            int oldRows = table.GetLength(1);
            string[,] newTable = new string[oldCols, oldRows + 1];
            for (int i = 0; i < oldCols; i++)
            {
                for (int j = 0; j < oldRows; j++)
                {
                    newTable[i, j] = table[i, j];
                }
                newTable[i, oldRows] = values[i];
            }
            table = newTable;
        }
    }
}
