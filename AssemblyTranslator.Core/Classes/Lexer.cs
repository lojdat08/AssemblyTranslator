using System;
using System.Globalization;

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
        if (IsDigit(lexem, out Int32 cislo)) token = new Constant(cislo);
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
        if (label[label.Length - 1] != ':') return false; // každé návěstí musí končit s ':'
        return true;
    }

    private bool IsDigit(string text, out Int32 cislo)
    {
        if(text.Length > 2)
        {
            bool negative = text.StartsWith("-");
            cislo = 0;

            if (text.Substring(negative ? 1 : 0, 2) == "0b")
            {
                foreach(char let in text.Substring((negative ? 1 : 0) + 2))
                {
                    if (let < '0' || let > '1') return false;
                    cislo <<= 1; // posune bity
                    cislo |= let == '1' ? 1 : 0; // nastaví jeden bit
                }

                if (negative) cislo = -cislo;
                return true;
            }
            else if(text.Substring(negative ? 1 : 0, 2) == "0x")
            {
                if (!UInt32.TryParse(text.Substring((negative ? 1 : 0) + 2), NumberStyles.AllowHexSpecifier, null, out UInt32 num)) return false;
                cislo = (Int32)num;
                if (negative) cislo = -cislo;
                return true;
            }
        }
        return Int32.TryParse(text, out cislo);
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

    public void ChangeTokensToAddresses(Token[] tokens)
    {
        for (int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].type == TokenType.constant)
            {
                tokens[i] = new Address((UInt32)((Constant)tokens[i]).value);
            }
        }
    }

    public void ChangeTokensToRelative(Token[] tokens, Address curAddress)
    {
        for(int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].type == TokenType.address)
            {
                tokens[i] = ((Address)tokens[i]).relative(curAddress);
            }
            else if (tokens[i].type == TokenType.constant)
            {
                tokens[i] = new RelativeAddress(((Constant)tokens[i]).value);
            }
        }
    }
}