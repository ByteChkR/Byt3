using System;
using System.IO;
using System.Linq;
using Byt3.Callbacks;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;
using Byt3.Utilities.FastString;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile
{
    public class SerializableFromCSVFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args, FLBufferModifiers modifiers,
            int arraySize)
        {
            if (!modifiers.IsArray)
            {
                throw new InvalidOperationException("Can not load a csv file as a texture.");
            }

            byte[] csvData = ParseCSV(args[0].Replace("\"", ""));
            return new SerializableFromBinaryFLBuffer(name, csvData, csvData.Length, 1, 1, modifiers);
        }

        private static byte[] ParseCSV(string file)
        {
            if (!IOManager.FileExists(file))
            {
                throw new FileNotFoundException("Can not find file:" + file);
            }

            return IOManager.ReadText(file).Pack(",").Select(x => x.Trim()).Select(byte.Parse).ToArray();
        }

        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "csv";
        }
    }
}