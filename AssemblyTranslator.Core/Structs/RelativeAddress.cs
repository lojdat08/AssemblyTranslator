using System;

public struct RelativeAddress
{
    public Int32 value;

    public RelativeAddress(Int32 value)
    {
        this.value = value;
    }

    public static bool TryParse(string input, out RelativeAddress relativeAddress)
    {
        relativeAddress = new RelativeAddress(0);
        return Int32.TryParse(input, out relativeAddress.value);
    }
}