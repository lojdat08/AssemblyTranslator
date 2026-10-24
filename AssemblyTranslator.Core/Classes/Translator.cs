using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

public class Translator
{
    Dictionary<string, Instruction> instructionMap;
    Dictionary<string, LongInstruction> longInstructionMap;
    Analyzer analyzer;

    public Translator()
    {
        analyzer = new Analyzer();
        instructionMap = new Dictionary<string, Instruction>();
        longInstructionMap = new Dictionary<string, LongInstruction>();

        ImportInstructions.SetInstructions(ref instructionMap);
        ImportInstructions.SetInstructions(ref longInstructionMap);
    }

    public bool TranslateCode(string[] inputData, out string[] outputData, out List<string> errorList)
    {
        analyzer.Reset();
        errorList = new List<string>();
        int lineCounter = 0;
        Address curAddress = new Address(0);

        ParseLabels(inputData, ref errorList, out UInt32 wordSize);
        UInt16[] opcodeData = new UInt16[wordSize];
        outputData = new string[wordSize];

        foreach (string line in inputData)
        {
            string[] textArgs = analyzer.GetArgs(line);
            if (textArgs.Length <= 0) continue;

            lineCounter++;

            if (IsValidInstruction(textArgs[0], out bool isLong))
            {
                object[] args = analyzer.ConvertArgs(textArgs);
                if (!isLong)
                {
                    Instruction instruction = instructionMap[textArgs[0]];
                    if (instruction.IsValid(args))
                    {
                        UInt16 opcode = instruction.GetOpcode(args);
                        opcodeData[curAddress] = opcode;
                        curAddress++;
                    }
                    else
                    {
                        errorList.Add($"Instruction: \"{instruction}\" doesn't have valid arguments, at line {lineCounter}.");
                        continue;
                    }
                }
                else
                {
                    LongInstruction longInstruction = longInstructionMap[textArgs[0]];
                    if (longInstruction.IsValid(args))
                    {
                        UInt32 opcode = longInstruction.GetOpcode(args);
                        opcodeData[curAddress] = (UInt16)(opcode >> 16); // prvně přidat vysoký bity
                        curAddress++;
                        opcodeData[curAddress] = (UInt16)(opcode & 0xFFFF); // potom přidat nižší bity
                        curAddress++;
                    }
                    else
                    {
                        errorList.Add($"Instruction: \"{longInstruction}\" doesn't have valid arguments, at line {lineCounter}.");
                        continue;
                    }
                }
            }
            else if (analyzer.IsLabel(textArgs[0])) continue;
            else
            {
                errorList.Add($"Found invalid instruction: \"{textArgs[0]}\" at line: {lineCounter}");
                continue;
            }
        }

        for(int i = 0; i < wordSize; i++)
        {
            outputData[i] = opcodeData[i].ToString("X4");
        }

        return errorList.Count == 0;
    }

    public string IsCodeValid(string[] inputData)
    {
        return string.Empty; // Možnost kontroly celého kódu
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

    private void ParseLabels(string[] inputData, ref List<string> errorList, out UInt32 wordSize)
    {
        Address curAddress = new Address(0);
        int lineCounter = 0;
        wordSize = 0;

        foreach (string line in inputData)
        {
            lineCounter++;
            string arg = analyzer.GetFirstArg(line);
            if (arg == string.Empty) continue;
            if(IsValidInstruction(arg, out bool isLong))
            {
                curAddress++;
                if (isLong) curAddress++;
            }
            else if(analyzer.IsValidLabel(arg))
            {
                arg = analyzer.RemoveLabelIdentifier(arg);
                if (!analyzer.IsLabel(arg)) analyzer.AddLabel(arg, curAddress);
                else errorList.Add($"Label: \"{arg}\" is already defined, at line {lineCounter}.");
            }
        }
        wordSize = curAddress.value;
    }
}