using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Instruction
{
    private string name;
    public bool isLong { get; private set; }

    public Instruction(string name, bool isLong = false)
    {
        this.name = name;
        this.isLong = isLong;
    }

    public abstract UInt16 GetOpcode(Token[] tokens);
    public virtual UInt32 GetLongOpcode(Token[] tokens)
    {
        throw new NotImplementedException();
    }
    public abstract bool IsValid(Token[] tokens);

    public override string ToString()
    {
        return name;
    }
}