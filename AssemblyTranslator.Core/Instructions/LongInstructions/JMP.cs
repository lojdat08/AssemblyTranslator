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

public class JMP : LongInstruction
{
    public const string instructionName = "JMP";
    public JMP() : base(instructionName) { }

    public override UInt32 GetOpcode(object[] args)
    {
        UInt32 valueK = ((Address)args[1]).value;

        UInt32 opcode = 0x940C0000; //1001 010k kkkk 110k
                                    //kkkk kkkk kkkk kkkk
        opcode |= ((valueK & 0x3E0000) << 3);
        opcode |= (valueK & 0x1FFFF);
        return opcode;
    }

    public override bool IsValid(object[] args)
    {
        if (args.Length != 2) return false;
        if (!(args[1] is Address)) return false;
        return ((Address)args[1]).value <= 4194303; // 2ˇ22 - 1
    }
}