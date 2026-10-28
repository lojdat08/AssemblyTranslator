using System;

public class Constant : Token
{
    public Int32 value { get; private set; }
    public Constant(Int32 value) : base(TokenType.constant)
    {
        this.value = value;
    }
}