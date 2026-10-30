using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SymbolTable
{
    public readonly Dictionary<string, Instruction> instructionMap;

    public SymbolTable()
    {
        instructionMap = new()
        {
            { ADD.instructionName, new ADD() },
            { JMP.instructionName, new JMP() },
            { RJMP.instructionName, new RJMP() },
            { MOV.instructionName, new MOV() },
            { LDI.instructionName, new LDI() },
            { NOP.instructionName, new NOP() }
        };
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