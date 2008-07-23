﻿using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldString : MessageField
    {
        public MessageFieldString(int tag, IFieldIO fieldIO): base(tag, fieldIO, WireType.String)
        { }

        protected override bool CanAppendWriteCore { get { return true; } }
        protected override bool CanAppendReadCore { get { return true; } }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadString", Type.EmptyTypes));
        }

        public override void AppendGuard(ILGenerator il, MethodInfo getMethod, Label done)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, getMethod);
            il.Emit(OpCodes.Brfalse, done);
        }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteString", new Type[] { typeof(string) }));
        }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadString();
        }
    }
}
