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
/// <para>Syntax:   ADD Rd, Rr  (Needs 2 args of type "Register")</para>
/// <para>Cycles:   1</para>
/// </remarks>

// - constraints
//  0 <= d <= 31
//  0 <= r <= 31
//
public class ADD : Instruction
{
    public const string instructionName = "ADD";
    public ADD() : base(instructionName) { }

    public override UInt16 GetOpcode(object[] args) 
    {
        Register registerD = (Register)args[1];
        Register registerR = (Register)args[2];

        UInt16 opcode = 0x0c00; //0000 11rd dddd rrrr
        opcode |= (UInt16)((registerD.value & 0x1F) << 4);
        opcode |= (UInt16)(registerR.value & 0xF);
        opcode |= (UInt16)((registerR.value & 0x10) << 5);
        return opcode;
    }

    public override bool IsValid(object[] args)
    {
        if (args.Length != 3) return false;
        if (!(args[1] is Register)) return false;
        if (!(args[2] is Register)) return false;

        return ((Register)args[1]).value < 32 && ((Register)args[2]).value < 32;
    }
}