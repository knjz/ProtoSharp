﻿using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ProtoSharp.Core
{
    public class UnknownFieldCollection : IEnumerable<UnknownField>
    {
        public int Count { get { return _fields.Count; } }

        public UnknownField this[int index]
        {
            get { return _fields[index]; }
        }

        public void Add(MessageTag tag, MessageReader reader) 
        {
            Add(UnknownField.Create(tag, reader));
        }

        public void Serialize(Stream stream)
        {
            Serialize(new MessageWriter(stream));
        }

        public void Serialize(MessageWriter writer)
        {
            _fields.ForEach(x => x.Serialize(writer));
        }

        internal void Add(UnknownField unknown)
        {
            _fields.Add(unknown);
        }

        public IEnumerator<UnknownField> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<UnknownField>).GetEnumerator();
        }

        List<UnknownField> _fields = new List<UnknownField>();
    }
}
