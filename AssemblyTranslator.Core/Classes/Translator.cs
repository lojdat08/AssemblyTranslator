using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

public class Translator
{
    SymbolTable symbolTable;
    Lexer lexer;

    public Translator()
    {
        symbolTable = new SymbolTable();
        lexer = new Lexer();
    }

    public bool TranslateCode(string[] inputData, out string[] outputData, out List<string> errorList, out string[] labelsList)
    {
        lexer.Reset();
        errorList = new List<string>();
        int lineCounter = 0;
        Address curAddress = new Address(0);

        ParseLabels(inputData, ref errorList, out UInt32 wordSize);
        UInt16[] opcodeData = new UInt16[wordSize];
        outputData = new string[wordSize];

        foreach (string line in inputData)
        {
            string[] lexems = lexer.GetLexems(line);
            if (lexems.Length <= 0) continue;

            lineCounter++;

            if (symbolTable.IsValidInstruction(lexems[0]))
            {
                if (!lexer.TryConvertLexemsToTokens(lexems, out Token[] tokens))
                {
                    errorList.Add($"Error when converting lexems to tokens, at line {lineCounter}");
                    continue;
                }

                Instruction instruction = symbolTable.instructionMap[lexems[0]];

                if ((instruction.instrucitonFlags & InstrucitonFlags.needsRelativeAddress) != 0) lexer.ChangeAddressesToRelative(tokens, curAddress);
                if (instruction.IsValid(tokens))
                {
                    if(!instruction.isLong)
                    {
                        UInt16 opcode = instruction.GetOpcode(tokens);
                        opcodeData[curAddress] = opcode;
                        curAddress++;
                    }
                    else
                    {
                        UInt32 opcode = instruction.GetLongOpcode(tokens);
                        opcodeData[curAddress] = (UInt16)(opcode >> 16); // prvně přidat vysoký bity
                        curAddress++;
                        opcodeData[curAddress] = (UInt16)(opcode & 0xFFFF); // potom přidat nižší bity
                        curAddress++;
                    }
                }
                else
                {
                    errorList.Add($"Instruction: \"{instruction}\" doesn't have valid arguments, at line {lineCounter}.");
                    continue;
                }
            }
            else if (lexer.IsLabel(lexems[0])) continue;
            else
            {
                errorList.Add($"Found unknown instruction: \"{lexems[0]}\" at line: {lineCounter}");
                continue;
            }
        }

        for(int i = 0; i < wordSize; i++)
        {
            outputData[i] = opcodeData[i].ToString("X4");
        }

        {
            labelsList = new string[lexer.labels.Count];
            int i = 0;
            foreach (KeyValuePair<String, Address> label in lexer.labels)
            {
                labelsList[i] = $"{label.Key} -> {label.Value}";
                i++;
            }
        }

        return errorList.Count == 0;
    }

    private void ParseLabels(string[] inputData, ref List<string> errorList, out UInt32 wordSize)
    {
        Address curAddress = new Address(0);
        int lineCounter = 0;
        wordSize = 0;

        foreach (string line in inputData)
        {
            lineCounter++;
            string lexem = lexer.GetFirstLexem(line);
            if (lexem == string.Empty) continue;
            if (symbolTable.IsValidInstruction(lexem, out bool isLong))
            {
                curAddress++;
                if (isLong) curAddress++;
            }
            else if(lexer.IsValidLabel(lexem))
            {
                lexem = lexer.RemoveLabelIdentifier(lexem);
                if (!lexer.IsLabel(lexem)) lexer.AddLabel(lexem, curAddress);
                else errorList.Add($"Label: \"{lexem}\" is already defined, at line {lineCounter}.");
            }
        }
        wordSize = curAddress.value;
    }
}