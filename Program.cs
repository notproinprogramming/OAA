using System.Runtime.InteropServices;

Console.WriteLine("Консольна програма");
string str = "", s;
char c = ' ';
while (true)
{
    s = Console.ReadLine();
    if (s != "" && s[s.Length - 1] == ';')
    {
        int counter = s.Length - 2;
        while (s[counter] == ' ')
        {
            s = s.Remove(counter, 1);
            counter--;
        }
        s = ignorespace(s);
        if (s == "exit;")
        {
            break;
        }
        str += s;
        Console.WriteLine(str);

        str = "";
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
    while (str[0] == ' ')
    {
        str = str.Remove(0, 1);
    }
    while (str[str.Length - 1] == ' ')
    {
        str = str.Remove(str.Length - 1, 1);
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
    string[] command = str.Split(' ');
    command[command.Length - 1] = command[command.Length - 1].Remove(s.Length - 1, 1);
    switch (command[0].ToLower())
    {
        case "create":
            {
                break;
            }
        case "insert":
            {
                if (command[1].ToLower() == "into")
                {
                    rem2comword(ref command);
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
        case "join":
            {
                break;
            }
        default:
            break;
    }
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
}