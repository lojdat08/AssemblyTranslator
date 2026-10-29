using System;
using System.Text;

public class Lexer
{
    public Dictionary<string, Address> labels { get; private set; }

    public Lexer()
    {
        labels = new Dictionary<string, Address>();
    }


    public void Reset()
    {
        labels.Clear();
    }

    public bool TryGetLabel(string label, out Address address)
    {
        return labels.TryGetValue(label, out address);
    }

    public string[] GetLexems(string line)
    {
        string curData = RemoveComments(line);
        curData = curData.Trim();
        if (curData == string.Empty) return Array.Empty<string>();

        return curData.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); // rozdělení na jednotlivé lexémy
    }

    public string GetFirstLexem(string line)
    {
        string newLine = RemoveComments(line);
        newLine = newLine.Trim();
        if (newLine == string.Empty) return string.Empty;

        string[] lexems = newLine.Split(new[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);
        if (lexems.Length > 0) return lexems[0];
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

    public bool TryConvertLexemsToTokens(string[] lexems, out Token[] tokens) // přeskočí první lexem (vždy název instrukce)
    {
        if (lexems.Length <= 1)
        {
            tokens = Array.Empty<Token>();
            return lexems.Length > 0;
        }
        tokens = new Token[lexems.Length - 1];

        for (int i = 0; i < lexems.Length - 1; i++)
        {
            if (TryConvertLexemToToken(lexems[i + 1], out Token token))
            {
                tokens[i] = token;
            }
            else return false;
        }
        return true;
    }

    private bool TryConvertLexemToToken(string lexem, out Token token) // regsitry, návěstí, konstanty
    {
        if (IsDigit(lexem, out int cislo)) token = new Constant(cislo);
        else if (IsLabel(lexem)) token = labels[RemoveLabelIdentifier(lexem)]; 
        else if (IsRegister(lexem, out Register register)) token = register;
        else
        {
            token = null!;
            return false;
        }
        return true;
    }

    public string RemoveLabelIdentifier(string label)
    {
        if (label[label.Length - 1] == ':') return label.Substring(0, label.Length - 1);
        else return label;
    }

    public bool IsValidLabel(string label)
    {
        if (label.Length < 2) return false;
        if (char.IsDigit(label[0])) return false; // kvůli konfliktům návěstí a konstant nesmí návěstí obsahovat čílsici na prvním místě
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

    public void ChangeAddressesToRelative(Token[] tokens, Address curAddress)
    {
        for(int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].type == TokenType.address)
            {
                tokens[i] = ((Address)tokens[i]).relative(curAddress);
            }
        }
    }
}