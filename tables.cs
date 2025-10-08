using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAA_1
{
    internal class tables
    {
        private string[,] table;
        private string name;
        private int[] indexed;

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

        public void InsertRow(string[] values)
        {
            if (values.Length != table.GetLength(0))
                throw new ArgumentException("Кількість значень не відповідає кількості стовпців.");

            int oldCols = table.GetLength(0);
            int oldRows = table.GetLength(1);

            string[,] newTable = new string[oldCols, oldRows + 1];
            for (int i = 0; i < oldCols; i++)
            {
                for (int j = 0; j < oldRows; j++)
                    newTable[i, j] = table[i, j];

                newTable[i, oldRows] = values[i];
            }

            table = newTable;
        }

        public void SelectFrom(string command)
        {
            string[] parts = command.Substring("select ".Length).Split(' ', 2);
            string targetTable = parts[0];

            if (targetTable != name)
            {
                Console.WriteLine($" Таблиця '{targetTable}' не знайдена (очікувалась '{name}')");
                return;
            }
            string rest = (parts.Length > 1) ? parts[1].Trim() : "";

            string condition = null;
            string order = null;

            if (rest.Contains("WHERE"))
            {
                int whereIndex = rest.IndexOf("WHERE");
                int orderIndex = rest.IndexOf("ORDER_BY");

                if (orderIndex != -1)
                {
                    condition = rest.Substring(whereIndex + 5, orderIndex - (whereIndex + 5)).Trim();
                    order = rest.Substring(orderIndex + 8).Trim().TrimEnd(';');
                }
                else
                {
                    condition = rest.Substring(whereIndex + 5).Trim().TrimEnd(';');
                }
            }
            else if (rest.Contains("ORDER_BY"))
            {
                int orderIndex = rest.IndexOf("ORDER_BY");
                order = rest.Substring(orderIndex + 8).Trim().TrimEnd(';');
            }

            int rows = table.GetLength(1);
            int cols = table.GetLength(0);
            List<string[]> data = new List<string[]>();
            for (int r = 1; r < rows; r++)
            {
                string[] row = new string[cols];
                for (int c = 0; c < cols; c++)
                    row[c] = table[c, r];
                data.Add(row);
            }

            if (condition != null)
                data = ApplyWhere(data, condition);

            if (order != null)
                data = ApplyOrderBy(data, order);

            PrintTable(data);
        }

        private List<string[]> ApplyWhere(List<string[]> data, string condition)
        {
            string[] ops = condition.Split('>');
            if (ops.Length != 2)
                return data;

            string left = ops[0].Trim();
            string right = ops[1].Trim().Trim('"');

            int leftIndex = GetColumnIndex(left);
            int rightIndex = GetColumnIndex(right);

            if (leftIndex == -1)
                return data;

            List<string[]> filtered = new List<string[]>();

            foreach (var row in data)
            {
                string leftVal = row[leftIndex];
                string rightVal = rightIndex != -1 ? row[rightIndex] : right;
                if (string.Compare(leftVal, rightVal, StringComparison.Ordinal) > 0)
                    filtered.Add(row);
            }

            return filtered;
        }

        private List<string[]> ApplyOrderBy(List<string[]> data, string order)
        {
            string[] orderParts = order.Split(',');

            // Зберігаємо список (стовпець, напрямок)
            var orders = new List<(int index, bool asc)>();
            foreach (string part in orderParts)
            {
                string[] temp = part.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string col = temp[0];
                bool asc = (temp.Length < 2 || temp[1].ToUpper() == "ASC");
                int idx = GetColumnIndex(col);
                if (idx != -1)
                    orders.Add((idx, asc));
            }

            // Каскадне сортування
            data = data.OrderBy(row => 0).ToList(); // щоб створити IOrderedEnumerable
            IOrderedEnumerable<string[]> sorted = null;

            for (int i = 0; i < orders.Count; i++)
            {
                int idx = orders[i].index;
                bool asc = orders[i].asc;
                if (i == 0)
                    sorted = asc
                        ? data.OrderBy(r => r[idx], StringComparer.Ordinal)
                        : data.OrderByDescending(r => r[idx], StringComparer.Ordinal);
                else
                    sorted = asc
                        ? sorted.ThenBy(r => r[idx], StringComparer.Ordinal)
                        : sorted.ThenByDescending(r => r[idx], StringComparer.Ordinal);
            }

            return sorted?.ToList() ?? data;
        }

        private int GetColumnIndex(string colName)
        {
            for (int i = 0; i < table.GetLength(0); i++)
                if (table[i, 0] == colName)
                    return i;
            return -1;
        }

        private void PrintTable(List<string[]> data)
        {
            int cols = table.GetLength(0);
            for (int i = 0; i < cols; i++)
                Console.Write($"{table[i, 0],-12}");
            Console.WriteLine();
            Console.WriteLine(new string('-', cols * 12));

            foreach (var row in data)
            {
                for (int i = 0; i < cols; i++)
                    Console.Write($"{row[i],-12}");
                Console.WriteLine();
            }
        }
    }
}