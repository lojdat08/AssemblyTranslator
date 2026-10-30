using System;

namespace ProjectTests.ValueTypes
{
    public class AddressTests
    {
        public static TheoryData<string, bool, UInt32> tryParseData => new()
        {
            { "", false, 0},
            { "0", true, 0},
            { UInt32.MaxValue.ToString(), true, UInt32.MaxValue},
            { ((UInt64)UInt32.MaxValue + 1).ToString(), false, 0},
            { "125543515255252", false, 0},
            { "-1", false, 0},
        };

        [Theory]
        [MemberData(nameof(tryParseData), DisableDiscoveryEnumeration = true)]
        public void TryParse(string input, bool expected, UInt32 expectedValue = 0)
        {
            if (Address.TryParse(input, out Address address))
            {
                if (!expected)
                {
                    Assert.Fail();
                    return;
                }
                Assert.Equal(address.value, expectedValue);
            }
            else Assert.False(expected);
        }

        public static TheoryData<Address, Address, RelativeAddress> relativeData => new()
        {
            { new Address(0), new Address(1), new RelativeAddress(1) },
            { new Address(10), new Address(5), new RelativeAddress(-5) },
            { new Address(50000), new Address(25000), new RelativeAddress(-25000) }
        };

        [Theory]
        [MemberData(nameof(relativeData))]
        public void Relative(Address curAddress, Address targetAddress, RelativeAddress expected)
        {
            Assert.Equal(expected.value, targetAddress.relative(curAddress).value);
        }
    }
}
