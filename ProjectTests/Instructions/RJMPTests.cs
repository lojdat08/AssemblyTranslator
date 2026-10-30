using System;

namespace ProjectTests.Instructions
{
    public class RJMPTests
    {
        [Fact]
        public void GetLongOpcode()
        {
            RJMP rjmp = new RJMP();
            Assert.Throws<NotImplementedException>(() => rjmp.GetLongOpcode(Array.Empty<Token>()));
        }


        public static TheoryData<Token[], UInt16> GetOpcodeData => new()
        {
            { new Token[] { new RelativeAddress(0) }, 0xC000 }, // RJMP 0
            { new Token[] { new RelativeAddress(-10) }, 0xCFF6 }, // RJMP -10
            { new Token[] { new RelativeAddress(10) }, 0xC00A }, // RJMP 10
            { new Token[] { new RelativeAddress(2047) }, 0xC7FF }, // RJMP 2047
            { new Token[] { new RelativeAddress(-2048) }, 0xC800 } // RJMP -2048
        };

        [Theory]
        [MemberData(nameof(GetOpcodeData))]
        public void GetOpcode(Token[] tokens, UInt16 expectedOpcode)
        {
            RJMP rjmp = new RJMP();
            Assert.Equal(expectedOpcode, rjmp.GetOpcode(tokens));
        }


        public static TheoryData<Token[], bool> IsValidData => new()
        {
            { new Token[] { }, false }, // RJMP - false
            { new Token[] { new RelativeAddress(0) }, true }, // RJMP 0 - true
            { new Token[] { new RelativeAddress(1024) }, true }, // RJMP 1024 - true
            { new Token[] { new RelativeAddress(-2048) }, true }, // RJMP -2048 - true
            { new Token[] { new RelativeAddress(-2049) }, false }, // RJMP 2049 - false
            { new Token[] { new RelativeAddress(2047) }, true }, // RJMP 2047 - true
            { new Token[] { new RelativeAddress(2048) }, false }, // RJMP 2048 - false
            { new Token[] { new RelativeAddress(10000000) }, false }, // RJMP 10000000 - false
            { new Token[] { new Address(0) }, false }, // (experimental)
            { new Token[] { new Constant(0) }, false } // (experimental)
        };

        [Theory]
        [MemberData(nameof(IsValidData))]
        public void IsValid(Token[] tokens, bool expected)
        {
            RJMP rjmp = new RJMP();
            Assert.Equal(expected, rjmp.IsValid(tokens));
        }
    }
}
