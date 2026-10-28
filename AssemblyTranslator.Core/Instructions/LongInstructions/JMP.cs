using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// JMP - Jump
/// </summary>
/// <remarks>
/// <para>Opeartion:   Program counter = k</para>
/// <para>Syntax:   JMP k</para>
/// <para>Cycles:   3</para>
/// </remarks>

// - constraints
//  0 <= k <= 4M (4 194 303)
//

public class JMP : Instruction
{
    public const string instructionName = "JMP";
    public JMP() : base(instructionName, true) { }

    public override ushort GetOpcode(Token[] tokens)
    {
        throw new NotImplementedException();
    }

    public override UInt32 GetLongOpcode(Token[] tokens)
    {
        UInt32 valueK = ((Address)tokens[0]).value;

        UInt32 opcode = 0x940C0000; //1001 010k kkkk 110k
                                    //kkkk kkkk kkkk kkkk
        opcode |= ((valueK & 0x3E0000) << 3);
        opcode |= (valueK & 0x1FFFF);
        return opcode;
    }

    public override bool IsValid(Token[] tokens)
    {
        if (tokens.Length != 1) return false;
        if (tokens[0].type != TokenType.address) return false;
        return ((Address)tokens[0]).value <= Address.maxValue; // 2ˇ22 - 1
    }
}