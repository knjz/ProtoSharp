﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core.UnknownFields
{
    class UnknownFieldGroup : UnknownField
    {
        public UnknownFieldGroup(MessageTag tag, MessageReader reader) : base(tag, ReadGroup(tag, reader)) { }

        static UnknownFieldCollection ReadGroup(MessageTag startTag, MessageReader reader)
        {
            var group = new UnknownFieldCollection();
            for(int stop = startTag.WithWireType(WireType.EndGroup), tag = reader.ReadInt32(); tag != stop; tag = reader.ReadInt32())
                group.Add(new MessageTag(tag), reader);
            return group;
        }

        protected override void SerializeCore(MessageWriter writer)
        {
            (Value as UnknownFieldCollection).Serialize(writer);
        }
    }
}
