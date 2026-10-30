/// <summary>
/// NOP - No Operation
/// </summary>
/// <remarks>
/// <para>Opeartion:   none/para>
/// <para>Syntax:   NOP</para>
/// </remarks>

public class NOP : Instruction
{
    public const string instructionName = "NOP";
    public NOP() : base(instructionName) { }

    public override UInt16 GetOpcode(Token[] tokens)
    {
        return 0;
    }

    public override bool IsValid(Token[] tokens)
    {
        return tokens.Length == 0;
    }
}