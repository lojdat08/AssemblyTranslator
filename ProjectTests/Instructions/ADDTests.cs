using System;

namespace ProjectTests.Instructions
{
    public class ADDTests
    {

        [Theory]
        [InlineData(new object[] { "ADD", "r0", "r0"}, (UInt16)0x0c00)]
        [InlineData(new object[] { "ADD", "r31", "r31" }, (UInt16)0x0fff)]
        [InlineData(new object[] { "ADD", "r16", "r16" }, (UInt16)0x0f00)]
        [InlineData(new object[] { "ADD", "r15", "r17" }, (UInt16)0x0ef1)]
        public void GetOpcode(object[] args, UInt16 expectedOpcode)
        {
            ADD add = new ADD();
            Register.TryParse((string)args[1], out Register register1);
            Register.TryParse((string)args[2], out Register register2);
            args[1] = register1;
            args[2] = register2;
            Assert.Equal(expectedOpcode, add.GetOpcode(args));
        }

        [Theory]
        [InlineData(new object[] { "ADD", "r0", "r0" }, true)]
        [InlineData(new object[] { "ADD" }, false)]
        [InlineData(new object[] { "ADD", "r0" }, false)]
        [InlineData(new object[] { "ADD", "r0", "r0", "r0" }, false)]
        [InlineData(new object[] { "ADD", "r31", "r31" }, true)]
        [InlineData(new object[] { "ADD", "r32", "r32" }, false)]
        public void IsValid(object[] args, bool expected)
        {
            ADD add = new ADD();
            for (int i = 1; i < args.Length; i++)
            {
                if(Register.TryParse((string)args[i], out Register register))
                {
                    args[i] = register;
                }
            }
            Assert.Equal(expected, add.IsValid(args));
        }
    }
}
