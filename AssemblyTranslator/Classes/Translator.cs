using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

class Translator
{
    Dictionary<string, Instruction> instructionMap;
    Dictionary<string, LongInstruction> longInstructionMap;

    public Translator()
    {
        instructionMap = new Dictionary<string, Instruction>();
        longInstructionMap = new Dictionary<string, LongInstruction>();

        ImportInstructions.SetInstructions(ref instructionMap);
        ImportInstructions.SetInstructions(ref longInstructionMap);
    }

    public bool TranslateCode(string[] inputData, out string[] outputData)
    {
        outputData = null;
        bool result = true;
        List<UInt16> opcodeData = new List<UInt16>();
        foreach (string line in inputData)
        {
            string curData = GetFormatedLine(line);
            if (curData == string.Empty) continue;

            string[] args = curData.Split(new[] { ' ', '\t'});
            RemoveEmptyArgs(ref args);

            MessageBox.Show($"Args[0]: {args[0]}");
            if (IsValidInstruction(args[0], out bool isLong))
            {
                if (!isLong)
                {
                    Instruction instruction = instructionMap[args[0]];

                    if (instruction.IsValid(args))
                    {
                        UInt16 opcode = instruction.GetOpcode(args);
                        opcodeData.Add(opcode);
                    }
                    else
                    {
                        MessageBox.Show($"Instruction ({args[0]}) is not valid");
                        return false;
                    }
                }
                else
                {
                    LongInstruction longInstruction = longInstructionMap[args[0]];

                    if (longInstruction.IsValid(args))
                    {
                        UInt32 opcode = longInstruction.GetOpcode(args);
                        opcodeData.Add((UInt16)(opcode >> 16));
                        opcodeData.Add((UInt16)(opcode & 0xFFFF));
                    }
                    else
                    {
                        MessageBox.Show($"Long Instruction ({args[0]}) is not valid");
                        return false;
                    }
                }
            }
            else return false;

            /*string temp = string.Empty;
            foreach (string arg in args)
            {
                temp += $" \"{arg}\"";
            }
            MessageBox.Show(temp);*/
        }

        List<string> outputDataList = new List<string>();
        foreach(UInt16 opcode in opcodeData)
        {
            outputDataList.Add(opcode.ToString("X4"));
        }
        outputData = outputDataList.ToArray();

        return result;
    }

    public string IsCodeValid(string[] inputData)
    {
        return string.Empty; // Možnost kontroly celého kódu
    }

    private string GetFormatedLine(string line)
    {
        string curData = string.Empty;
        foreach (char letter in line)
        {
            if (letter == '#') break;   //remove comments
            curData += letter;
        }
        return curData;
    }

    private void RemoveEmptyArgs(ref string[] args)
    {
        List<String> newArgs = new List<string>();
        foreach (string arg in args)
        {
            if (arg != string.Empty) newArgs.Add(arg);
        }
        args = newArgs.ToArray();
    }

    private bool IsValidInstruction(string instructionName, out bool isLong)
    {
        isLong = false;
        if (instructionMap.ContainsKey(instructionName)) return true;
        if (longInstructionMap.ContainsKey(instructionName))
        {
            isLong = true;
            return true;
        }
        return false;
    }
}