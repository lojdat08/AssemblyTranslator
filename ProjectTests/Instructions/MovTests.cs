using System;

namespace ProjectTests.Instructions
{
    public class MOVTests
    {
        public static TheoryData<Token[], UInt16> GetOpcodeData => new()
        {
            { new Token[] { new Register(0), new Register(0) }, 0x2c00 },   // MOV r0, r0
            { new Token[] { new Register(31), new Register(31) }, 0x2fff }, // MOV r31, r31
            { new Token[] { new Register(16), new Register(16) }, 0x2f00 }, // MOV r16, r16
            { new Token[] { new Register(15), new Register(17) }, 0x2ef1 }  // MOV r15, r17
        };

        [Theory]
        [MemberData(nameof(GetOpcodeData))]
        public void GetOpcode(Token[] tokens, UInt16 expectedOpcode)
        {
            MOV mov = new MOV();
            Assert.Equal(expectedOpcode, mov.GetOpcode(tokens));
        }

        [Fact]
        public void GetLongOpcode()
        {
            MOV mov = new MOV();
            Assert.Throws<NotImplementedException>(() => mov.GetLongOpcode(Array.Empty<Token>()));
        }

        public static TheoryData<Token[], bool> IsValidData => new()
        {
            { new Token[] { new Register(0), new Register(0) }, true }, // MOV r0, r0 - true
            { new Token[] { }, false }, // MOV - false
            { new Token[] { new Register(0) }, false }, // MOV r0 - false
            { new Token[] { new Register(0), new Register(0), new Register(0) }, false }, // MOV r0, r0, r0 - false
            { new Token[] { new Register(31), new Register(31) }, true }, // MOV r31, r31 - true
            { new Token[] { new Register(32), new Register(32) }, false },  // MOV r32, r32 - false
            { new Token[] { new Register(0), new Address(0) }, false }  // MOV r0, 0 - false (experimental)
        };

        [Theory]
        [MemberData(nameof(IsValidData))]
        public void IsValid(Token[] args, bool expected)
        {
            MOV mov = new MOV();
            Assert.Equal(expected, mov.IsValid(args));
        }
    }
}
