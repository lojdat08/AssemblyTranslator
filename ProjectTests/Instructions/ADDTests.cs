using System;

namespace ProjectTests.Instructions
{
    public class ADDTests
    {
        public static TheoryData<Token[], UInt16> GetOpcodeData => new()
        {
            { new Token[] { new Register(0), new Register(0) }, 0x0c00 },   // ADD r0, r0
            { new Token[] { new Register(31), new Register(31) }, 0x0fff }, // ADD r31, r31
            { new Token[] { new Register(16), new Register(16) }, 0x0f00 }, // ADD r16, r16
            { new Token[] { new Register(15), new Register(17) }, 0x0ef1 }  // ADD r15, r17
        };

        [Theory]
        [MemberData(nameof(GetOpcodeData))]
        public void GetOpcode(Token[] tokens, UInt16 expectedOpcode)
        {
            ADD add = new ADD();
            Assert.Equal(expectedOpcode, add.GetOpcode(tokens));
        }

        [Fact]
        public void GetLongOpcode()
        {
            ADD add = new ADD();
            Assert.Throws<NotImplementedException>(() => add.GetLongOpcode(Array.Empty<Token>()));
        }

        public static TheoryData<Token[], bool> IsValidData => new()
        {
            { new Token[] { new Register(0), new Register(0) }, true }, // ADD r0, r0 - true
            { new Token[] { }, false }, // ADD - false
            { new Token[] { new Register(0) }, false }, // ADD r0 - false
            { new Token[] { new Register(0), new Register(0), new Register(0) }, false }, // ADD r0, r0, r0 - false
            { new Token[] { new Register(31), new Register(31) }, true }, // ADD r31, r31 - true
            { new Token[] { new Register(32), new Register(32) }, false },  // ADD r32, r32 - false
            { new Token[] { new Register(0), new Address(0) }, false }  // ADD r0, 0 - false (experimental)
        };

        [Theory]
        [MemberData(nameof(IsValidData))]
        public void IsValid(Token[] args, bool expected)
        {
            ADD add = new ADD();
            Assert.Equal(expected, add.IsValid(args));
        }
    }
}
