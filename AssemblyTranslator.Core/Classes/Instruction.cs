using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Instruction
{
    public string name { get; private set; }

    public Instruction(string name)
    {
        this.name = name;
    }

    public abstract UInt16 GetOpcode(object[] args);
    public abstract bool IsValid(object[] args);

    public override string ToString()
    {
        return name;
    }
}