using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Instruction
{
    public string name;
    public int cycles;

    public Instruction(string name, int cycles = 1)
    {
        this.name = name;
        this.cycles = cycles;
    }

    public abstract UInt16 GetOpcode(string[] args);
    public abstract bool IsValid(string[] args);
}