﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Runtime.Serialization;

namespace Dopple.InstructionNodes
{
    [DataContract]
    public class NewObjectNode : InstructionNode
    {
        public int ConstructorParamCount { get; set; }
        public NewObjectNode(Instruction instruction, MethodDefinition method) : base(instruction, method)
        {
            StackPopCount = ((MethodReference) instruction.Operand).Parameters.Count;
            ConstructorParamCount = StackPopCount;
        }
    }
}
