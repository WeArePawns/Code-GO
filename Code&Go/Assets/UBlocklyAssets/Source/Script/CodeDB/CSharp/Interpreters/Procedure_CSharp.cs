﻿/****************************************************************************

Functions for interpreting c# code for blocks.

Copyright 2016 sophieml1989@gmail.com

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

****************************************************************************/


using System.Collections;

namespace UBlockly
{

    [CodeInterpreter(BlockType = "procedures_callreturn")]
    public class Procedure_CallReturn_Cmdtor : ControlCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            string procedureName = block.GetFieldValue("NAME");
            Block defBlock = block.Workspace.ProcedureDB.GetDefinitionBlock(procedureName);
            if (CheckInfiniteLoop()) yield return null;
            yield return CSharp.Interpreter.StatementRun(defBlock, "STACK");
            
            CustomEnumerator ctor = CSharp.Interpreter.ValueReturn(defBlock, "RETURN");
            yield return ctor;
            ReturnData(ctor.Data);
        }
    }

    [CodeInterpreter(BlockType = "procedures_callnoreturn")]
    public class Procedure_CallNoReturn_Cmdtor : ControlCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            string procedureName = block.GetFieldValue("NAME");
            Block defBlock = block.Workspace.ProcedureDB.GetDefinitionBlock(procedureName);
            if (CheckInfiniteLoop()) yield return null;
            yield return CSharp.Interpreter.StatementRun(defBlock, "STACK");
        }
    }

    [CodeInterpreter(BlockType = "procedures_ifreturn")]
    public class Proceudre_IfReturn_Cmdtor : ControlCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            if (CheckInfiniteLoop()) yield return null;
            yield return 0;
        }
    }
}
