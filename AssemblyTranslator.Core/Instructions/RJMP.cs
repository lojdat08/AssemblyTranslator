/// <summary>
/// RJMP - Realtive jump
/// </summary>
/// <remarks>
/// <para>Opeartion:   PC = PC + k + 1</para>
/// <para>Syntax:   RJMP start  (Needs 1 token of type "Reative address")</para>
/// </remarks>

// - constraints
//  -2048 <= k <= 2047

public class RJMP : Instruction
{
    public const string instructionName = "RJMP";
    public RJMP() : base(instructionName, instrucitonFlags: InstrucitonFlags.needsRelativeAddress) { }

    public override UInt16 GetOpcode(Token[] tokens)
    {
        RelativeAddress relativeAddress = (RelativeAddress)tokens[0];

        UInt16 opcode = 0xC000; //1100 0000 0000 0000
        opcode |= (UInt16)(relativeAddress.value & 0x0FFF);

        return opcode;
    }

    public override bool IsValid(Token[] tokens)
    {
        if (tokens.Length != 1) return false;
        if (tokens[0].type != TokenType.relativeAddress) return false;

        return ((RelativeAddress)tokens[0]).value >= -2048 && ((RelativeAddress)tokens[0]).value <= 2047;
    }
}