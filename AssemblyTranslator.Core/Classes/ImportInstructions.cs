using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ImportInstructions
{
    public static void SetInstructions(ref Dictionary<string, Instruction> instructions)
    {
        instructions.Add(ADD.instructionName, new ADD());
    }

    public static void SetInstructions(ref Dictionary<string, LongInstruction> longInstructions)
    {
        longInstructions.Add(JMP.instructionName, new JMP());
    }
}