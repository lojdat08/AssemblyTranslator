using System;

namespace ProjectTests.ValueTypes
{
    public class AddressTests
    {
        [Theory]
        [InlineData("", false)]
        [InlineData("0", true, 0)]
        [InlineData("4194303", true, 4194303)]
        [InlineData("125543515255252", false)]
        [InlineData("-1", false)]

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
    }
}
