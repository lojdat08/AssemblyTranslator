using System;

namespace ProjectTests.Instructions
{
    public class JMPTests
    {
        [Fact]
        public void GetOpcode()
        {
            JMP jmp = new JMP();
            Assert.Throws<NotImplementedException>(() => jmp.GetOpcode(Array.Empty<Token>()));
        }


        public static TheoryData<Token[], UInt32> GetLongOpcodeData => new()
        {
            { new Token[] { new Address(0) }, 0x940C0000 }, // JMP 0
            { new Token[] { new Address(1032) }, 0x940C0408 }, // JMP 1032
            { new Token[] { new Address(4194303) }, 0x95FDFFFF } // JMP 4194303
        };

        [Theory]
        [MemberData(nameof(GetLongOpcodeData))]
        public void GetLongOpcode(Token[] tokens, UInt32 expectedOpcode)
        {
            JMP jmp = new JMP();
            Assert.Equal(expectedOpcode, jmp.GetLongOpcode(tokens));
        }


        public static TheoryData<Token[], bool> IsValidData => new()
        {
            { new Token[] { }, false }, // JMP - false
            { new Token[] { new Address(0) }, true }, // JMP 0 - true
            { new Token[] { new Address(1024) }, true }, // JMP 1024 - true
            { new Token[] { new Address(Address.maxValue) }, true }, // JMP 4194303 - true
            { new Token[] { new Address(Address.maxValue + 1) }, false }, // JMP 4194304 - false
            { new Token[] { new Address(10000000) }, false }, // JMP 10000000 - false
            { new Token[] { new Constant(0) }, false } // (experimental)
        };

        [Theory]
        [MemberData(nameof(IsValidData))]
        public void IsValid(Token[] tokens, bool expected)
        {
            JMP jmp = new JMP();
            Assert.Equal(expected, jmp.IsValid(tokens));
        }
    }
}
