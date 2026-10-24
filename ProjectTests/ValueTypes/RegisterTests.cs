using System;

namespace ProjectTests.ValueTypes
{
    public class RegisterTests
    {
        [Theory]
        [InlineData("", false)]
        [InlineData("r0", true, 0)]
        [InlineData("r31", true, 31)]
        [InlineData("r32", false)]
        [InlineData("r32,", false)]
        [InlineData("r12,", true, 12)]
        [InlineData("5", false)]

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
