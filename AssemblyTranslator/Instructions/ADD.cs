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
    public ADD() : base("ADD", 1, ArgTypes.arg2) { }

    public UInt16 GetOpcode(byte registerD, byte registerR)
    {
        UInt16 opcode = 0x1c00 ; //0b0001 11rd dddd rrrr
        opcode |= (UInt16)((registerD & 0x1F) << 4);
        opcode |= (UInt16)(registerR & 0xF);
        opcode |= (UInt16)((registerR & 0x10) << 9);
        return opcode;
    }

    public bool Validate(string name, byte registerD, byte registerR)
    {
        return this.name == name && registerD < 32 && registerR < 32;
    }

    public bool Validate(byte registerD, byte registerR)
    {
        return registerD < 32 && registerR < 32;
    }
}