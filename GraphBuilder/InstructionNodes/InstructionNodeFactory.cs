﻿using Dopple.InstructionNodes;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;

namespace Dopple.InstructionNodes
{
    public class InstructionNodeFactory
    {
        SystemMethodsLoader systemMethodsLoader = new SystemMethodsLoader();
        public InstructionNode[] GetInstructionNodes(Instruction instruction, MethodDefinition method)
        {
            Code nodeCode = instruction.OpCode.Code;
            if (CodeGroups.CallCodes.Contains(nodeCode))
            {
                MethodDefinition systemMethodDef = null;
                if (instruction.Operand is MethodDefinition)
                {
                    var targetMethodDef = (MethodDefinition) instruction.Operand;
                    //TODO remove again.;
                    if (targetMethodDef.FullName.Contains("Console"))
                    {
                        return new[] { new NonInlineableCallInstructionNode(instruction, method) };
                    }
                    if (targetMethodDef.IsVirtual)
                    {
                        return new[] { new VirtualCallInstructionNode(instruction, method) };
                    }
                    if (!targetMethodDef.HasBody)
                    {
                        return new[] { new NonInlineableCallInstructionNode(instruction, method) };
                    }
                    return new[] { new InlineableCallNode(instruction, method) };
                }
                else if (systemMethodsLoader.TryGetSystemMethod(instruction, out systemMethodDef))
                {
                    return new[] { new InlineableCallNode(instruction, systemMethodDef, method) };
                }
                else
                {
                    return new[] { new NonInlineableCallInstructionNode(instruction, method) };
                }
            }
            else if (nodeCode == Code.Callvirt)
            {
                return new[] { new VirtualCallInstructionNode(instruction, method) };
            }
            else if (nodeCode == Code.Newobj)
            {
                MethodDefinition constructorMethodDef = null;
                if (instruction.Operand is MethodDefinition)
                {
                    constructorMethodDef = (MethodDefinition)instruction.Operand;
                }
                else if (instruction.Operand is MethodReference)
                {
                    constructorMethodDef = ((MethodReference) instruction.Operand).Resolve();
                }
                if ((constructorMethodDef != null  || systemMethodsLoader.TryGetSystemMethod(instruction, out constructorMethodDef)) && constructorMethodDef.HasBody)
                {
                    var noArgsNewObject = new NewObjectNodeWithConstructor(instruction, method);
                    var constructorCallInst = Instruction.Create(CodeGroups.AllOpcodes.First(x => x.Code == Code.Call),(MethodReference)instruction.Operand);
                    constructorCallInst.Operand = instruction.Operand;
                    constructorCallInst.Next = instruction.Next;
                    var constructorCall = new ConstructorCallNode(constructorCallInst, constructorMethodDef, method, noArgsNewObject);
                    constructorCall.ProgramFlowBackRoutes.AddTwoWay(noArgsNewObject);
                    noArgsNewObject.ProgramFlowResolveDone = true;
                   
                    return new InstructionNode[] { noArgsNewObject, constructorCall };
                }
                else
                {
                    return new[] { new NewObjectNode(instruction, method) };
                }
            }
            else if (CodeGroups.LdArgCodes.Contains(nodeCode))
            {
                return new[] { new LdArgInstructionNode(instruction, method) };
            }
            else if (CodeGroups.LdArgAddressCodes.Contains(nodeCode))
            {
                return new[] { new LoadArgAddressInstructionNode(instruction, method) };
            }
            else if (CodeGroups.StArgCodes.Contains(nodeCode))
            {
                return new[] { new StoreArgumentNode(instruction, method) };
            }
            else if (CodeGroups.LdLocCodes.Contains(nodeCode))
            {
                return new[] { new LocationLoadInstructionNode(instruction, method) };
            }
            else if (CodeGroups.StLocCodes.Contains(nodeCode))
            {
                return new[] { new LocationStoreInstructionNode(instruction, method) };
            }
            else if (CodeGroups.LdLocAddressCodes.Contains(nodeCode))
            {
                return new[] { new LocationAddressLoadInstructionNode(instruction, method) };
            }
            else if (CodeGroups.LdImmediateFromOperandCodes.Concat(CodeGroups.LdImmediateValueCodes).Contains(nodeCode))
            {
                return new[] { new LdImmediateInstNode(instruction, method) };
            }
            else if (CodeGroups.LdElemCodes.Contains(nodeCode))
            {
                return new[] { new LdElemInstructionNode(instruction, method) };
            }
            else if (nodeCode == Code.Ret)
            {
                return new[] { new RetInstructionNode(instruction, method) };
            }
            else if (CodeGroups.CondJumpCodes.Contains(nodeCode))
            {
                return new[] { new ConditionalJumpNode(instruction, method) };
            }
            else if (CodeGroups.StIndCodes.Contains(nodeCode))
            {
                return new[] { new StIndInstructionNode(instruction, method) };
            }
            else if (CodeGroups.LdIndCodes.Contains(nodeCode))
            {
                return new[] { new LdindNode(instruction, method) };
            }
            else if (nodeCode == Code.Ldftn)
            {
                return new[] { new LoadFunctionNode(instruction, method) };
            }
            else if (CodeGroups.ArithmeticCodes.Contains(nodeCode))
            {
                return new[] { new ArithmaticsNode(instruction, method) };
            }
            else if (new []{ Code.Castclass, Code.Isinst}.Contains(nodeCode))
            {
                return new[] {new SingleIndexDataTransferNode(instruction, method) };
            }
            else if (CodeGroups.LoadFieldCodes.Contains(nodeCode))
            {
                return new[] { new LoadFieldNode(instruction, method) };
            }
            else if (CodeGroups.StoreFieldCodes.Contains(nodeCode))
            {
                return new[] { new StoreFieldNode(instruction, method) };
            }
            else if (CodeGroups.StElemCodes.Contains(nodeCode))
            {
                return new[] { new StElemInstructionNode(instruction, method) };
            }
            else if (nodeCode == Code.Dup)
            {
                return new[] { new DataTransferNode(instruction, method) };
            }
            else if (nodeCode == Code.Ldelema)
            {
                return new[] { new LdElemAddressNode(instruction, method) };
            }
            else if (nodeCode == Code.Ldsfld)
            {
                return new[] { new LoadStaticFieldNode(instruction, method) };
            }
            else if (nodeCode == Code.Stsfld)
            {
                return new[] { new StoreStaticFieldNode(instruction, method) };
            }
            return new[] {new InstructionNode(instruction, method)};
        }

        internal InstructionNode GetSingleInstructionNode(Instruction inst, MethodDefinition metDef)
        {
            return GetInstructionNodes(inst, metDef).First();
        }
    }
}