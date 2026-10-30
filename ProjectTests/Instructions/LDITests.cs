using System;

namespace ProjectTests.Instructions
{
    public class LDITests
    {
        public static TheoryData<Token[], UInt16> GetOpcodeData => new()
        {
            { new Token[] { new Register(16), new Constant(0) }, 0xE000 },   // LDI r16, 0
            { new Token[] { new Register(31), new Constant(255) }, 0xEFFF }, // LDI r31, 255
            { new Token[] { new Register(18), new Constant(16) }, 0xE120 }, // LDI r18, 16
            { new Token[] { new Register(20), new Constant(17) }, 0xE141 }  // LDI r15, 17
        };

        [Theory]
        [MemberData(nameof(GetOpcodeData))]
        public void GetOpcode(Token[] tokens, UInt16 expectedOpcode)
        {
            LDI ldi = new LDI();
            Assert.Equal(expectedOpcode, ldi.GetOpcode(tokens));
        }

        [Fact]
        public void GetLongOpcode()
        {
            LDI ldi = new LDI();
            Assert.Throws<NotImplementedException>(() => ldi.GetLongOpcode(Array.Empty<Token>()));
        }

        public static TheoryData<Token[], bool> IsValidData => new()
        {
            { new Token[] { new Register(16), new Constant(0) }, true }, // LDI r0, 0 - true
            { new Token[] { }, false }, // LDI - false
            { new Token[] { new Register(16) }, false }, // LDI r16 - false
            { new Token[] { new Register(16), new Constant(16), new Register(16) }, false }, // LDI r16, 16, r16 - false
            { new Token[] { new Register(31), new Constant(31) }, true }, // LDI r31, 31 - true
            { new Token[] { new Register(32), new Constant(32) }, false },  // LDI r32, 32 - false
            { new Token[] { new Register(0), new Constant(0) }, false },  // LDI r0, 0 - false
            { new Token[] { new Register(0), new Address(0) }, false }  // LDI r0, 0 - false (experimental)
        };

        [Theory]
        [MemberData(nameof(IsValidData))]
        public void IsValid(Token[] args, bool expected)
        {
            LDI ldi = new LDI();
            Assert.Equal(expected, ldi.IsValid(args));
        }
    }
}
