using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Instruction
{
    public string name;
    public int cycles;
    public ArgTypes argTypes;
    protected bool longInstruction { get; private set; }

    public Instruction(string name, int cycles, ArgTypes argTypes, bool longInstruction = false)
    {
        this.name = name;
        this.cycles = cycles;
        this.argTypes = argTypes;
        this.longInstruction = longInstruction;
    }
}