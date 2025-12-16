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
        public int[] indexed;

        public tables(string tablename, string[] tableparam/*, int[] ifindexed*/)
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
        public void SelectFrom(string command)
        {
            command = command.Trim();
            if (command.Length > 0 && command[command.Length - 1] == ';')
            {
                command = command.Remove(command.Length - 1, 1);
                command = command.Trim();
            }

            string afterSelect = command.Substring("select ".Length).Trim();
            if (afterSelect.StartsWith("from ", StringComparison.OrdinalIgnoreCase))
            {
                afterSelect = afterSelect.Substring(4).Trim();
            }

            string[] parts = afterSelect.Split(' ', 2);
            string targetTable = parts[0].Trim();

            if (targetTable != name)
            {
                Console.WriteLine($" Таблиця '{targetTable}' не знайдена (очікувалась '{name}')");
                return;
            }

            string rest = (parts.Length > 1) ? parts[1].Trim() : "";

            string condition = null;
            string order = null;

            int whereIndex = rest.IndexOf("where", StringComparison.OrdinalIgnoreCase);
            int orderIndex = rest.IndexOf("order_by", StringComparison.OrdinalIgnoreCase);

            if (whereIndex != -1)
            {
                if (orderIndex != -1)
                {
                    condition = rest.Substring(whereIndex + 5, orderIndex - (whereIndex + 5)).Trim();
                    order = rest.Substring(orderIndex + 8).Trim();
                }
                else
                {
                    condition = rest.Substring(whereIndex + 5).Trim();
                }
            }
            else if (orderIndex != -1)
            {
                order = rest.Substring(orderIndex + 8).Trim();
            }

            if (condition != null)
            {
                condition = condition.Trim().TrimEnd(';').Trim();
            }
            if (order != null)
            {
                order = order.Trim().TrimEnd(';').Trim();
            }

            int rows = table.GetLength(1);
            int cols = table.GetLength(0);

            List<string[]> data = new List<string[]>();

            for (int r = 1; r < rows; r++)
            {
                string[] row = new string[cols];
                for (int c = 0; c < cols; c++)
                {
                    row[c] = table[c, r];
                }
                data.Add(row);
            }

            if (condition != null && condition != "")
            {
                data = ApplyWhere(data, condition);
            }

            if (order != null && order != "")
            {
                data = ApplyOrderBy(data, order);
            }

            PrintTable(data);
        }

        private int FindCharOutsideQuotes(string s, char ch)
        {
            bool q = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '"')
                {
                    q = !q;
                    continue;
                }
                if (!q && s[i] == ch)
                {
                    return i;
                }
            }
            return -1;
        }

        private List<string[]> ApplyWhere(List<string[]> data, string condition)
        {
            int op = FindCharOutsideQuotes(condition, '>');
            if (op == -1)
                return data;

            string left = condition.Substring(0, op).Trim();
            string right = condition.Substring(op + 1).Trim().TrimEnd(';').Trim();

            if (right.Length > 1 && right[0] == '(' && right[right.Length - 1] == ')')
            {
                right = right.Substring(1, right.Length - 2).Trim();
            }

            if (right.Length >= 2 && right[0] == '"' && right[right.Length - 1] == '"')
            {
                right = right.Substring(1, right.Length - 2);
            }

            int leftIndex = GetColumnIndex(left);
            int rightIndex = GetColumnIndex(right);

            if (leftIndex == -1)
                return data;

            List<string[]> filtered = new List<string[]>();

            foreach (var row in data)
            {
                string leftVal = row[leftIndex] ?? "";
                string rightVal = rightIndex != -1 ? (row[rightIndex] ?? "") : (right ?? "");
                if (string.Compare(leftVal, rightVal, StringComparison.Ordinal) > 0)
                    filtered.Add(row);
            }

            return filtered;
        }

        private List<string[]> ApplyOrderBy(List<string[]> data, string order)
        {
            string[] orderParts = order.Split(',');

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

            data = data.OrderBy(row => 0).ToList(); 
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