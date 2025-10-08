using OAA_1;
using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Консольна програма");
string str = "", s;
char c = ' ';
tables[] tables = new tables[0];
while (true)
{
    s = Console.ReadLine();
    if (s != "" && s[s.Length - 1] == ';')
    {
        int counter = s.Length - 2;
        if (s.Length > counter)
        {
            while (counter > -1 &&  s[counter] == ' ')
            {
                s = s.Remove(counter, 1);
                counter--;
            }
        }
        s = ignorespace(s);
        if (s == "exit;")
        {
            break;
        }
        str += s;
        if (str.Length > 1 && str[str.Length - 1] == ';' && str[str.Length - 2] == ' ')
        {
            str = str.Remove(str.Length - 2, 1);
        }
        Console.WriteLine(str);
        if (str == "exit;")
        {
            break;
        }
        SeekFunc(str);
        str = "";
    }
    else if (str == "" || str[str.Length - 1] != ' ')
    {
        str += ignorespace(s) + " ";
    }
    else
    {
        str += ignorespace(s);
    }
}
string ignorespace(string str)
{
    for (int i = 0; i < str.Length; i++)
    {
        if (str[i] == '\t')
        {
            str = str.Remove(i, 1);
            str = str.Insert(i, " ");
        }
    }
    if (str.Length > 0)
    {
        while (str[0] == ' ')
        {
            str = str.Remove(0, 1);
        }
        while (str[str.Length - 1] == ' ')
        {
            str = str.Remove(str.Length - 1, 1);
        }
    }
    for (int i = 0; i < str.Length - 1; i++)
    {
        if (str[i] == ' ')
        {
            while (str[i + 1] == ' ')
            {
                str = str.Remove(i + 1, 1);
            }
        }
    }
    return str;
}


void SeekFunc(string str)
{
    //string[] command = str.Split(' ');
    //if (command[command.Length - 1] == ";")
    //{ 
    //    string [] newcommand = new string[command.Length - 1];
    //    for (int i = 0; i < command.Length - 1; i++)
    //    {
    //        newcommand[i] = command[i];
    //    }
    //    command = newcommand;
    //    goto skip1;
    //}
    ////command[command.Length - 1] = command[command.Length - 1].Remove(command[command.Length - 1].Length - 1, 1);
    //skip1:
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
                for (int j = 0; j < com2[i].Length; j++)
                {
                    if (com2[i][j] == ' ')
                    {
                        com2[i] = com2[i].Remove(j, 1);
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
                Console.WriteLine("Таблицю створено успішно");
                Console.WriteLine(tables[tables.Length - 1].name);
                    for (int i = 0; i < tables[tables.Length - 1].table.GetLength(0); i++)
                    {
                        Console.WriteLine(tables[tables.Length - 1].table[i, 0]);
                    }
                    Console.WriteLine(param.Length);
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
                        tables[k].InsertRow(data);
                        Console.WriteLine("Рядок додано");
                    }
                    else
                    {
                        Console.WriteLine("Таблиці не існує");
                    }
                    break;
            }
        case "select":
            {
                if (command[1].ToLower() == "from")
                {
                    rem2comword(ref command);
                }
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
        Console.WriteLine("Шукаю таблицю за назваою: " + name);
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
        
    }
    //string[,] table = tables[k].table;
    //int len = table.GetLength(0);
    //int rows = table.GetLength(1);
    //int count = 0;
    //for (int i = 0; i < rows; i++)
    //{
    //    if (i < r.Length && r[i])
    //    {
    //        count++;
    //    }
    //}
    //int[] maxlen = new int[len];
    //string[,] newtable = tables[k].table;
    //for (int i = 0; i < len; i++)
    //{
    //    maxlen[i] = table[i, 0].Length;
    //    for (int j = 1; j < len; j++)
    //    {
    //        if (j < r.Length && r[j])
    //        {
    //            if (table[i, j].Length > maxlen[i])
    //            {
    //                maxlen[i] = table[i, j].Length;
    //            }
    //            newtable[i, j] = table[i, j];
    //        }
    //    }
    //}
    //table = newtable;
    //string section = "+";
    //for (int i = 0; i < len; i++)
    //{
    //    for (int j = 0; j < maxlen[i] + 2; j++)
    //    {
    //        section += "-";
    //    }
    //    section += "+";
    //}
    //string str;
    //for (int i = 0; i < table.GetLength(1); i++)
    //{
    //    str = "|";
    //    Console.WriteLine(section);
    //    for (int j = 0; j < table.GetLength(0); j++)
    //    {
    //        str += " " + table[i, j] + new string(' ', maxlen[i] - table[i, j].Length) + " |";
    //    }
    //    Console.WriteLine(str);

    //}
    //Console.WriteLine(section);
}