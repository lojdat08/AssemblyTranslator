using System;

public class RelativeAddress : Token
{
    public Int32 value;

    public RelativeAddress(Int32 value) : base(TokenType.relativeAddress)
    {
        this.value = value;
    }

    public RelativeAddress(Address curAddress, Address targetAddress) : base(TokenType.relativeAddress)
    {
        this.value = (Int32)targetAddress.value - (Int32)curAddress.value;
    }

    public static bool TryParse(string input, out RelativeAddress relativeAddress)
    {
        relativeAddress = new RelativeAddress(0);
        return Int32.TryParse(input, out relativeAddress.value);
    }

    public override string ToString()
    {
        return value.ToString();
    }
}