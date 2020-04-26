using System;
using System.Collections.Generic;
using System.Reflection;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators
{
    public class BufferCreator
    {
        private readonly List<ASerializableBufferCreator> bufferCreators = new List<ASerializableBufferCreator>();


        public static BufferCreator CreateWithBuiltInTypes()
        {
            BufferCreator bc = new BufferCreator();
            bc.AddBufferCreatorsInAssembly(Assembly.GetExecutingAssembly());
            return bc;
        }

        public void AddBufferCreator(ASerializableBufferCreator bufferCreator)
        {
            bufferCreators.Add(bufferCreator);
        }

        public void AddBufferCreatorsInAssembly(Assembly asm)
        {
            Type[] ts = asm.GetExportedTypes();

            Type target = typeof(ASerializableBufferCreator);

            for (int i = 0; i < ts.Length; i++)
            {
                if (target != ts[i] && target.IsAssignableFrom(ts[i]))
                {
                    ASerializableBufferCreator bc = (ASerializableBufferCreator) Activator.CreateInstance(ts[i]);
                    AddBufferCreator(bc);
                }
            }
        }

        public SerializableFLBuffer Create(string key, string name, string[] arguments)
        {
            for (int i = 0; i < bufferCreators.Count; i++)
            {
                if (bufferCreators[i].IsCorrectBuffer(key))
                {
                    return bufferCreators[i].CreateBuffer(name, arguments);
                }
            }

            throw new FLBufferCreatorNotFoundException("Can not find buffercreator with key: " + key);
        }
    }
}