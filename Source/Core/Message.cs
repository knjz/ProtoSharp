﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ProtoSharp.Core
{
    public static class Message
    {
        public static void ForEachField(Type messageType, Action<MessageField> action)
        {
            Array.ForEach(messageType.GetProperties(), field =>
            {
                var attributes = field.GetCustomAttributes(typeof(TagAttribute), false);
                if(attributes.Length == 0)
                    return;
                var tag = attributes[0] as TagAttribute;
                action(MessageField.Create(tag, field));
            });
        }

        public static int CountFields(object obj)
        {
            int count = 0;
            ForEachField(obj.GetType(), x => ++count);
            return count;
        }

        public static T CreateDefault<T>() where T : class, new()
        {
            return CreateDefault(new T()) as T;
        }

        public static T CreateDefaultItem<T>(string s)
        {
            return (T)CreateDefaultItem(typeof(T), s);
        }

        static object CreateDefault(object obj)
        {
            Array.ForEach(obj.GetType().GetProperties(), item =>
            {
                var attributes = item.GetCustomAttributes(typeof(DefaultAttribute), false);
                if(attributes.Length == 0)
                    return;
                var defaultAttribute = attributes[0] as DefaultAttribute;
                item.SetValue(obj, CreateDefaultItem(item.PropertyType, defaultAttribute.Value), null);
            });
            return obj;
        }
        static object CreateDefaultItem(Type type, string s)
        {
            var stringMethodArg = new Type[] { typeof(string) };

            var parse = type.GetMethod("Parse", stringMethodArg);
            if(parse != null)
                return parse.Invoke(null, new object[] { s });

            var tryParse = type.GetMethod("TryParse", new Type[]{ typeof(string), type.MakeByRefType() });
            if(tryParse != null)
            {
                var args = new object[] { s, null };
                if((bool)tryParse.Invoke(null, args))
                    return args[1];
            }

            var constructor = type.GetConstructor(stringMethodArg);
            if(constructor != null)
                return constructor.Invoke(new object[] { s });
            return null;
        }
    }
}
