using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class LongInstruction
{
    public string name { get; private set; }

    public LongInstruction(string name)
    {
        this.name = name;
    }

    public abstract UInt32 GetOpcode(object[] args);
    public abstract bool IsValid(object[] args);
}