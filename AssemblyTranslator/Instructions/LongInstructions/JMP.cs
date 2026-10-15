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

class JMP : LongInstruction
{
    public const string instructionName = "JMP";
    public JMP() : base(instructionName) { }

    public override UInt32 GetOpcode(string[] args)
    {
        if (!UInt32.TryParse(args[1], out UInt32 valueK)) return 0;

        UInt32 opcode = 0x940C0000; //1001 010k kkkk 110k
                                    //kkkk kkkk kkkk kkkk
        opcode |= ((valueK & 0x3E0000) << 3);
        opcode |= (valueK & 0x1FFFF);
        return opcode;
    }

    public override bool IsValid(string[] args)
    {
        if (args.Length != 2) return false;
        if (!UInt32.TryParse(args[1], out UInt32 valueK)) return false;
        return valueK <= 4194303; // 2ˇ22 - 1
    }
}