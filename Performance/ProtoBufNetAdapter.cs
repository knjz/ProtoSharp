﻿using ProtoBuf;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    class ProtoBufNetAdapter : BenchmarkAdapterBase
    {
        public ProtoBufNetAdapter(byte[] memory)
            : base(memory)
        { }

        public override void Serialize(MessageWithInt32 item){ Serializer.Serialize(Memory, item); }
        public override void Serialize(MessageWithUInt32 item) { Serializer.Serialize(Memory, item); }
    }
}