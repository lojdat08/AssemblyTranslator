using System;

namespace ProjectTests.ValueTypes
{
    public class RegisterTests
    {
        public static TheoryData<string, bool, byte> TryParseData => new()
        {
            { "", false, 0 },
            { "r0", true, 0 },
            { "r31", true, 31 },
            { "r32", false, 0 },
            { "r31,", true, 31 },
            { "r32,", false, 0 },
            { "r12,", true, 12 },
            { "5", false, 0 },
        };

        [Theory]
        [MemberData(nameof(TryParseData), DisableDiscoveryEnumeration = true)]
        public void TryParse(string input, bool expected, byte expectedValue = 0)
        {
            if(Register.TryParse(input, out Register register))
            {
                if (!expected)
                {
                    Assert.Fail();
                    return;
                }
                Assert.Equal(expectedValue, register.value);
            }
            else
            {
                Assert.False(expected);
            }
        }
    }
}
