using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// ADD - Add without Carry
/// </summary>
/// <remarks>
/// <para>Opeartion:   Rd = Rd + Rr</para>
/// <para>Syntax:   ADD Rd, Rr</para>
/// <para>Cycles:   1</para>
/// </remarks>

// - constraints
//  0 <= d <= 31
//  0 <= r <= 31
//
class ADD : Instruction
{
    public const string instructionName = "ADD";
    public ADD() : base(instructionName) { }

    public override UInt16 GetOpcode(string[] args)
    {
        if (!UInt16.TryParse(args[1].Substring(1), out UInt16 registerD)) return 0;
        if (!UInt16.TryParse(args[2].Substring(1), out UInt16 registerR)) return 0;

        UInt16 opcode = 0x0c00; //0000 11rd dddd rrrr
        opcode |= (UInt16)((registerD & 0x1F) << 4);
        opcode |= (UInt16)(registerR & 0xF);
        opcode |= (UInt16)((registerR & (0x10 >> 4)) << 9);
        return opcode;
    }

    public override bool IsValid(string[] args)
    {
        if (args.Length != 3) return false;
        if (!UInt16.TryParse(args[1].Substring(1), out UInt16 registerD)) return false;
        if (!UInt16.TryParse(args[2].Substring(1), out UInt16 registerR)) return false;
        return registerD < 32 && registerR < 32;
    }
}