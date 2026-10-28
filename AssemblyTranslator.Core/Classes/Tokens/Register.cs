using System;

public class Register : Token
{
    public const byte maxValue = 31;
    public byte value;

    public Register(byte value) : base(TokenType.register)
    {
        this.value = value;
    }

    public static bool TryParse(string input, out Register register)
    {
        register = new Register(0);
        if (input == null) return false;
        if (input == string.Empty) return false;
        if (input.Length < 2) return false;
        if (input[0] != 'r') return false;
        byte value;

        if (input[input.Length - 1] == ',')
        {
            if (!byte.TryParse(input.Substring(1, input.Length - 2), out value)) return false;
        }
        else
        {
            if (!byte.TryParse(input.Substring(1), out value)) return false;
        }

        if (value >= 32) return false;

        register.value = value;
        return true;
    }
}