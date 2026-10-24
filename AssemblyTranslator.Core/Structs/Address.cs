using System;
using System.Runtime.CompilerServices;

public struct Address
{
    public const UInt32 maxValue = 4194303; // 2ˇ22 - 1
    public UInt32 value;

    public Address(UInt32 value)
    {
        this.value = value;
    }

    public RelativeAddress relative(Address targetAddress)
    {
        return new RelativeAddress((Int32)targetAddress.value - (Int32)value);
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
        if (UInt32.TryParse(input, out UInt32 value))
        {
            if (address.value <= maxValue)
            {
                address.value = value;
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}