/// <summary>
/// LDI - Load Immediate
/// </summary>
/// <remarks>
/// <para>Opeartion:   Rd = K/para>
/// <para>Syntax:   LDI Rd, K  (Needs 2 tokens of type "Register" and type "Constant")</para>
/// </remarks>

// - constraints
//  16 <= d <= 31
//  0 <= k <= 255
//
public class LDI : Instruction
{
    public const string instructionName = "LDI";
    public LDI() : base(instructionName) { }

    public override UInt16 GetOpcode(Token[] tokens)
    {
        Register registerD = (Register)tokens[0];
        Constant valueK = (Constant)tokens[1];

        UInt16 opcode = 0xE000; //1110 KKKK dddd KKKK
        opcode |= (UInt16)((registerD.value & 0xF) << 4);
        opcode |= (UInt16)(valueK.value & 0xF);
        opcode |= (UInt16)((valueK.value & 0xF0) << 4);
        return opcode;
    }

    public override bool IsValid(Token[] tokens)
    {
        if (tokens.Length != 2) return false;
        if (tokens[0].type != TokenType.register) return false;
        if (tokens[1].type != TokenType.constant) return false;

        Register registerD = (Register)tokens[0];
        Constant valueK = (Constant)tokens[1];

        return registerD.value >= 16 && registerD.value <= 31 && valueK.value <= 255;
    }
}