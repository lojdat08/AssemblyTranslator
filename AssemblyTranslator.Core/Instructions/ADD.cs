/// <summary>
/// ADD - Add without Carry
/// </summary>
/// <remarks>
/// <para>Opeartion:   Rd = Rd + Rr</para>
/// <para>Syntax:   ADD Rd, Rr  (Needs 2 tokens of type "Register")</para>
/// </remarks>

// - constraints
//  0 <= d <= 31
//  0 <= r <= 31
//
public class ADD : Instruction
{
    public const string instructionName = "ADD";
    public ADD() : base(instructionName) { }

    public override UInt16 GetOpcode(Token[] tokens) 
    {
        Register registerD = (Register)tokens[0];
        Register registerR = (Register)tokens[1];

        UInt16 opcode = 0x0c00; //0000 11rd dddd rrrr
        opcode |= (UInt16)((registerD.value & 0x1F) << 4);
        opcode |= (UInt16)(registerR.value & 0xF);
        opcode |= (UInt16)((registerR.value & 0x10) << 5);
        return opcode;
    }

    public override bool IsValid(Token[] tokens)
    {
        if (tokens.Length != 2) return false;
        if (tokens[0].type != TokenType.register) return false;
        if (tokens[1].type != TokenType.register) return false;

        return ((Register)tokens[0]).value < 32 && ((Register)tokens[1]).value < 32;
    }
}