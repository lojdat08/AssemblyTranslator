using System;
using System.Runtime.CompilerServices;

public class Address : Token
{
    public UInt32 value;

    public Address(UInt32 value) : base(TokenType.address)
    {
        this.value = value;
    }

    public RelativeAddress relative(Address curAddress)
    {
        return new RelativeAddress((Int32)value - (Int32)curAddress.value);
    }

    public static Address operator ++(Address address)
    {
        return new Address(address.value + 1);
    }

    public static Address operator +(Address address, UInt32 num)
    {
        return new Address(address.value + num);
    }

    public static implicit operator UInt32(Address address)
    {
        return address.value;
    }


    public static bool TryParse(string input, out Address address)
    {
        address = new Address(0);
        return UInt32.TryParse(input, out address.value);
    }

    public override string ToString()
    {
        return value.ToString();
    }
}