using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.ElementModifiers
{
    public class FLBufferModifiers : FLElementModifiers
    {
        public FLBufferModifiers(string functionName, string[] modifiers) : base(functionName, modifiers)
        {
        }

        protected override string[] ValidKeywords => new[]
        {
            FLKeywords.ArrayKey, FLKeywords.TextureKey, FLKeywords.ReadOnlyBufferModifier,
            FLKeywords.ReadWriteBufferModifier, FLKeywords.InitializeOnStartKey
        };

        public virtual bool IsReadOnly => Modifiers.Contains(FLKeywords.ReadOnlyBufferModifier);
        public virtual bool IsReadWrite => Modifiers.Contains(FLKeywords.ReadWriteBufferModifier);
        public virtual bool IsArray => Modifiers.Contains(FLKeywords.ArrayKey);
        public virtual bool IsTexture => Modifiers.Contains(FLKeywords.TextureKey);
        public virtual bool InitializeOnStart => Modifiers.Contains(FLKeywords.InitializeOnStartKey);

        protected override void Validate()
        {
            if (IsArray && IsTexture)
            {
                throw new FLInvalidFLElementModifierUseException(ElementName, "array/texture",
                    "Can not declare a buffer of type texture and array.");
            }

            if (IsReadOnly && IsReadWrite)
            {
                throw new FLInvalidFLElementModifierUseException(ElementName, "readonly/readwrite",
                    "Can not declare a buffer readonly and readwrite.");
            }

            if (!IsReadOnly && !IsReadWrite)
            {
                Modifiers.Insert(1, FLKeywords.ReadWriteBufferModifier);
            }
        }
    }
}