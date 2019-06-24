﻿using Dopple.InstructionNodes;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dopple.VerifierNs
{
    class ArithmeticsVerifier : Verifier
    {
        public ArithmeticsVerifier(List<InstructionNode> instructionNodes) : base(instructionNodes)
        {
        }

        public override void Verify(InstructionNode instructionWrapper)
        {
            Code[] handledCodes = new[] 
                    {Code.Add, Code.Add_Ovf, Code.Add_Ovf_Un,
                     Code.Sub, Code.Sub_Ovf, Code.Sub_Ovf_Un,
                     Code.Div, Code.Div_Un,
                     Code.Mul, Code.Mul_Ovf, Code.Mul_Ovf_Un};

            if (!handledCodes.Contains(instructionWrapper.Instruction.OpCode.Code))
            {
                return;
            }
            foreach (var arg in instructionWrapper.DataFlowBackRelated)
            {
                if (BacktraceStLdLoc(arg.Argument).All(x => IsProvidingNumber(x)))
                {

                }
                else
                {

                }
            }
        }
    }
}

