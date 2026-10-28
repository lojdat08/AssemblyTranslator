using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SymbolTable
{
    public Dictionary<string, Instruction> instructionMap { get; private set; }

    public SymbolTable()
    {
        instructionMap = new Dictionary<string, Instruction>();

        instructionMap.Add(ADD.instructionName, new ADD());
        instructionMap.Add(JMP.instructionName, new JMP());
    }

    public bool IsValidInstruction(string instructionName)
    {
        return instructionMap.ContainsKey(instructionName);
    }

    public bool IsValidInstruction(string instructionName, out bool isLong)
    {
        if (instructionMap.ContainsKey(instructionName))
        {
            isLong = instructionMap[instructionName].isLong;
            return true;
        }
        else
        {
            isLong = false;
            return false;
        }
    }
}