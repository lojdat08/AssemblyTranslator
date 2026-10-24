using System;
using System.Text;

public class Analyzer
{
    private Dictionary<string, Address> labels;

    public Analyzer()
    {
        labels = new Dictionary<string, Address>();
    }


    public void Reset()
    {
        labels.Clear();
    }

    public bool TryGetLavel(string label, out Address address)
    {
        return labels.TryGetValue(label, out address);
    }

    public string[] GetArgs(string line)
    {
        string curData = RemoveComments(line);
        curData = curData.Trim();
        if (curData == string.Empty) return Array.Empty<string>();

        string[] args = curData.Split(new[] { ' ', '\t' });
        RemoveEmptyArgs(ref args);

        return args;
    }

    public string GetFirstArg(string line)
    {
        string curData = RemoveComments(line);
        curData = curData.Trim();
        if (curData == string.Empty) return string.Empty;

        string[] args = curData.Split(new[] { ' ', '\t' });
        foreach (string arg in args)
        {
            if (arg != string.Empty) return arg;
        }
        return string.Empty;
    }

    public void AddLabel(string label, Address address)
    {
        labels.Add(label, address);
    }

    public bool TryAddLabel(string label, Address address)
    {
        if(IsValidLabel(label))
        {
            if (!IsLabel(label))
            {
                label = RemoveLabelIdentifier(label);
                labels.Add(label, address);
                return true;
            }
        }
        return false;
    }

    public bool IsLabel(string label)
    {
        if (label[label.Length - 1] == ':')
        {
            return labels.ContainsKey(label.Substring(0, label.Length - 1));
        }
        else return labels.ContainsKey(label);
    }

    public object[] ConvertArgs(string[] args) // regsitry, návěstí, konstanty
    {
        object[] newArgs = new object[args.Length];
        if (args.Length <= 0) return newArgs;
        newArgs[0] = args[0];

        for (int i = 1; i < args.Length && i < 3; i++)
        {
            if (IsLabel(args[i])) newArgs[i] = labels[args[i]];
            else if (IsDigit(args[i], out int cislo)) newArgs[i] = new Constant(cislo);
            else if (IsRegister(args[i], out Register register)) newArgs[i] = register;
            else newArgs[i] = "";
        }
        return newArgs;
    }

    public string RemoveLabelIdentifier(string label)
    {
        if (label[label.Length - 1] == ':') return label.Substring(0, label.Length - 1);
        else return label;
    }

    public bool IsValidLabel(string label)
    {
        if (label.Length < 2) return false;
        if (label[label.Length - 1] != ':') return false; // každé návěstí musí končit s ':'
        return true;
    }

    private bool IsDigit(string text, out int cislo)
    {
        return int.TryParse(text, out cislo);
    }

    private bool IsRegister(string text, out Register register)
    {
        return Register.TryParse(text, out register);
    }

    private string RemoveComments(string line)
    {
        int stopIndex = line.Length;
        for (int i = 0; i < line.Length; i++)
        {
            if(line[i] == '#')
            {
                stopIndex = i;
                break;
            }
        }
        if (stopIndex == line.Length) return line;
        else return line.Substring(0, stopIndex);
    }

    private void RemoveEmptyArgs(ref string[] args)
    {
        List<String> newArgs = new List<string>();
        foreach (string arg in args)
        {
            if (arg != string.Empty) newArgs.Add(arg);
        }
        args = newArgs.ToArray();
    }
}