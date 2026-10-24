using System;

namespace ProjectTests.Instructions
{
    public class JMPTests
    {

        [Theory]
        [InlineData(new object[] { "JMP", "0" }, (UInt32)0x940C0000)]
        [InlineData(new object[] { "JMP", "1032" }, (UInt32)0x940C0408)]
        [InlineData(new object[] { "JMP", "4194303" }, (UInt32)0x95FDFFFF)]
        public void GetOpcode(object[] args, UInt32 expectedOpcode)
        {
            JMP jmp = new JMP();
            Address.TryParse((string)args[1], out Address address);
            args[1] = address;
            Assert.Equal(expectedOpcode, jmp.GetOpcode(args));
        }

        [Theory]
        [InlineData(new object[] { "JMP", "" }, false)]
        [InlineData(new object[] { "JMP", "0" }, true)]
        [InlineData(new object[] { "JMP", "-1" }, false)]
        [InlineData(new object[] { "JMP", "1024" }, true)]
        [InlineData(new object[] { "JMP", "4194303" }, true)]
        [InlineData(new object[] { "JMP", "10000000" }, false)]
        [InlineData(new object[] { "JMP", "999999999999" }, false)]
        public void IsValid(object[] args, bool expected)
        {
            JMP jmp = new JMP();
            for (int i = 1; i < args.Length; i++)
            {
                if (Address.TryParse((string)args[i], out Address address))
                {
                    args[i] = address;
                }
            }
            Assert.Equal(expected, jmp.IsValid(args));
        }
    }
}
