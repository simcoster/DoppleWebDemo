﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dopple.InstructionNodes
{
    [DataContract]
    public class NewObjectNodeWithConstructor : InstructionNode
    {
        public NewObjectNodeWithConstructor(Instruction instruction, MethodDefinition method) : base(instruction, method)
        {
            StackPopCount = 0;
            StackPushCount = 0;
        }
    }
}
