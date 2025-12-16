using OAA_1;
using System.Text;
Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Консольна програма");
string str = "", s;
tables[] tables = new tables[0];
while (true)
{
    s = Console.ReadLine();
    if (s == null)
    {
        break;
    }
    int sc = FindSemicolonOutsideQuotes(s);
    if (sc != -1)
    {
        string part = s.Substring(0, sc + 1);
        part = ignorespace(part);
        if (str == "")
        {
            str = part;
        }
        else
        {
            str += " " + part;
            str = ignorespace(str);
        }
        if (str.ToLower() == "exit;")
        {
            break;
        }
        SeekFunc(str);
        str = "";
    }
    else
    {
        string part = ignorespace(s);
        if (part != "")
        {
            if (str == "")
            {
                str = part;
            }
            else
            {
                str += " " + part;
            }
        }
    }
}
int FindSemicolonOutsideQuotes(string s)
{
    bool q = false;
    for (int i = 0; i < s.Length; i++)
    {
        if (s[i] == '"')
        {
            q = !q;
            continue;
        }
        if (!q && s[i] == ';')
        {
            return i;
        }
    }
    return -1;
}
string ignorespace(string str)
{
    StringBuilder sb = new StringBuilder();
    bool q = false;
    bool sp = false;
    for (int i = 0; i < str.Length; i++)
    {
        char ch = str[i];
        if (ch == '"')
        {
            q = !q;
            sb.Append(ch);
            sp = false;
            continue;
        }
        if (q)
        {
            sb.Append(ch);
            continue;
        }
        if (ch == '\t')
        {
            ch = ' ';
        }
        if (ch == ' ')
        {
            if (sb.Length == 0)
            {
                continue;
            }
            if (sp)
            {
                continue;
            }
            sb.Append(' ');
            sp = true;
            continue;
        }
        if ((ch == ';' || ch == ',' || ch == ')') && sb.Length > 0 && sb[sb.Length - 1] == ' ')
        {
            sb.Remove(sb.Length - 1, 1);
        }
        sb.Append(ch);
        sp = false;
    }
    while (sb.Length > 0 && sb[sb.Length - 1] == ' ')
    {
        sb.Remove(sb.Length - 1, 1);
    }
    return sb.ToString();
}

void SeekFunc(string str)
{
    string str1 = str;
    string[] command = new string[0];
    //try
    //{
        if (str.Split('(').Length == 2)
        {
            str = str.Remove(str.Length - 2, 2);
            string[] semicommand = str.Split('(');
        if (semicommand[1][0] == ' ')
        {
            semicommand[1].Remove(0, 1);
        }
        if (semicommand[0][semicommand[0].Length - 1] == ' ')
        {
            semicommand[0].Remove(semicommand[0].Length - 1, 1);
        }
    string[] com1 = semicommand[0].Split(' ');
            string[] com2 = semicommand[1].Split(',');
        for (int i = 0; i < com2.Length; i++)
        {
            bool q = false;
            for (int j = 0; j < com2[i].Length; j++)
            {
                if (com2[i][j] == '"')
                {
                    q = !q;
                    continue;
                }
                if (!q && com2[i][j] == ' ')
                {
                    com2[i] = com2[i].Remove(j, 1);
                    j--;
                }
            }
        }
        command = new string[com1.Length + com2.Length];
            for (int i = 0; i < com1.Length; i++)
            {
                command[i] = com1[i];
            }
            for (int i = 0; i < com2.Length; i++)
            {
                command[i + com1.Length] = com2[i];
            }
        }
        else
        {
            if (str.Split('(').Length == 1)
            {
                str.Remove(str.Length - 1, 1);
                command = str.Split(' ');
            }
            else
            {
                Console.WriteLine("Некоректна команда");
                return;
            }
        }
    //}
    //catch
    //{
    //    Console.WriteLine("Некоректна команда");
    //}
    //try
    //{ 
    switch (command[0].ToLower())
    {
        case "create":
            {
                for (int i = 0; i < tables.Length; i++)
                {
                    if (tables[i].name == command[1])
                    {
                        Console.WriteLine($"Таблиця з таким ім'ям вже існує.");
                        return;
                    }
                }
                tables[] newtables = new tables[tables.Length + 1];
                for (int i = 0; i < tables.Length; i++)
                {
                    newtables[i] = tables[i];
                }
                    string[] param = new string[command.Length - 3];
                for (int i = 3; i < command.Length; i++)
                {
                    param[i - 3] = command[i];
                }
                if (param[param.Length - 1][param[param.Length - 1].Length - 1] == ')')
                    {
                        param[param.Length - 1] = param[param.Length - 1].Remove(param[param.Length - 1].Length - 1, 1);
                    }
                newtables[newtables.Length - 1] = new tables(command[1], param);
                tables = newtables;
                Console.WriteLine($"Таблицю {tables[tables.Length - 1].name} створено успішно");
                break;
            }
        case "insert":
            {
                if (command[1].ToLower() == "into")
                {
                    rem2comword(ref command);
                }
                int k = NameToIndex(command[1]);
                    string[] data = new string[command.Length - 3];
                    for (int i = 3; i < command.Length; i++)
                    {
                        data[i - 3] = command[i];
                    data[i - 3] = data[i - 3].Remove(0, 1);
                    data[i - 3] = data[i - 3].Remove(data[i - 3].Length - 1, 1);
                    }
                    if (k != -1)
                    {
                    if (data.Length == tables[k].table.GetLength(0))
                    {
                        tables[k].InsertRow(data);
                        Console.WriteLine("Рядок додано.");
                    }
                    else
                    {
                        Console.WriteLine("Кількість аргументів не співпадає із кількістю стовпців.");
                    }
                    }
                    else
                    {
                        Console.WriteLine("Таблиці не існує.");
                    }
                    break;
            }
        case "select":
            {
                string s = str1.Trim();
                if (s.Length > 0 && s[s.Length - 1] == ';')
                {
                    s = s.Remove(s.Length - 1, 1);
                    s = s.Trim();
                }
                string low = s.ToLower();
                if (low.StartsWith("select"))
                {
                    s = s.Substring(6).Trim();
                    low = s.ToLower();
                }
                if (low.StartsWith("from"))
                {
                    s = s.Substring(4).Trim();
                }
                if (s.Length == 0)
                {
                    Console.WriteLine("Некоректна команда");
                    break;
                }
                int sp = s.IndexOf(' ');
                string tab = sp == -1 ? s : s.Substring(0, sp);
                string tail = sp == -1 ? "" : s.Substring(sp).Trim();
                int k = NameToIndex(tab);
                if (k == -1)
                {
                    Console.WriteLine("Таблиці не існує.");
                    break;
                }
                string q = "select " + tab;
                if (tail.Length > 0)
                {
                    q += " " + tail;
                }
                q += ";";
                tables[k].SelectFrom(q);
                break;
            }
        case "print":
            {
                command[1] = command[1].Remove(command[1].Length - 1, 1);
                if (NameToIndex(command[1]) != -1)
                {
                    int k = NameToIndex(command[1]);
                    bool[] b = new bool[tables[k].table.GetLength(1)];
                    for (int i = 0; i < b.Length; i++)
                    {
                        b[i] = true;
                    }
                    PrintTable(k, b);
                }
                else
                {
                    Console.WriteLine("Такої таблиці не існує");
                }
                break;
            }
        default:
            break;
    }
    //} 
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("Підчас виконання команди сталася помилка" + ex);
    //    }
    void rem2comword(ref string[] command)
    {
        string[] newcommand = new string[command.Length - 1];
        newcommand[0] = command[0];
        for (int i = 1; i < command.Length - 1; i++)
        {
            newcommand[i] = command[i + 1];
        }
        command = newcommand;
    }
    int NameToIndex(string name)
    {
        for (int i = 0; i < tables.Length; i++)
        {
            if (tables[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }
    void PrintTable(int k, bool[] r)
    {
        string[,] table = tables[k].table;
        int len = table.GetLength(0);
        int rows = table.GetLength(1);
        int count = 0;
        for (int i = 0; i < rows; i++)
        {
            if (i == 0 || (i < r.Length && r[i]))
            {
                count++;
            }
        }
        int[] maxlen = new int[len];
        string[,] newtable = tables[k].table;
        for (int i = 0; i < len; i++)
        {
            string cell = table[i, 0] ?? "";
            maxlen[i] = cell.Length;
            for (int j = 1; j < rows; j++)
            {
                if (!(j < r.Length && r[j]))
                {
                    continue;
                }
                cell = table[i, j] ?? "";
                if (cell.Length > maxlen[i])
                {
                    maxlen[i] = cell.Length;
                }
                newtable[i, j] = table[i, j];
            }
        }
        table = newtable;
        string section = "+";
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < maxlen[i] + 2; j++)
            {
                section += "-";
            }
            section += "+";
        }
        string str;
        for (int i = 0; i < rows; i++)
        {
            if (!(i == 0 || (i < r.Length && r[i])))
            {
                continue;
            }
            str = "|";
            Console.WriteLine(section);
            for (int j = 0; j < len; j++)
            {
                string cell = table[j, i] ?? "";
                str += " " + cell + new string(' ', maxlen[j] - cell.Length) + " |";
            }
            Console.WriteLine(str);
        }
        Console.WriteLine(section);
    }
}