using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.ElementModifiers
{
    public class FLFunctionElementModifiers : FLExecutableElementModifiers
    {
        public FLFunctionElementModifiers(string functionName, string[] modifiers) : base(functionName, modifiers)
        {
            if (functionName == FLKeywords.EntryFunctionKey)
            {
                Modifiers.Clear();
                Modifiers.Add(FLKeywords.NoJumpKeyword);
                Modifiers.Add(FLKeywords.NoCallKeyword);
                Modifiers.Add(FLKeywords.DynamicElementModifier);
            }
        }

        protected override string[] ValidKeywords => new[]
        {
            FLKeywords.StaticElementModifier, FLKeywords.DynamicElementModifier, FLKeywords.NoJumpKeyword,
            FLKeywords.NoCallKeyword, FLKeywords.ComputeOnceKeyword, FLKeywords.InitializeOnStartKey
        };

        public virtual bool IsStatic => Modifiers.Contains(FLKeywords.StaticElementModifier);
        public virtual bool IsDynamic => Modifiers.Contains(FLKeywords.DynamicElementModifier);

        protected override void Validate()
        {
            base.Validate();


            if (IsDynamic && IsStatic)
            {
                throw new FLInvalidFLElementModifierUseException(ElementName, "dynamic/static",
                    "Can not declare a function static and dynamic.");
            }

            if (ElementName != FLKeywords.EntryFunctionKey && NoCall && NoJump)
            {
                throw new FLInvalidFLElementModifierUseException(ElementName, "nocall/nojump",
                    $"The function {ElementName} is not reachable because nocall and nojump are set.");
            }

            if (InitializeOnStart && !IsStatic)
            {
                Modifiers.Add(FLKeywords.StaticElementModifier);
                if (!NoJump)
                {
                    Modifiers.Add(FLKeywords.NoJumpKeyword);
                }
            }

            //if ((EvaluateOnce || Init) && IsDynamic)
            //{
            //    throw new FLInvalidFLElementModifierUseException(ElementName, "dynamic/static", "Can not declare a function to be dynamic and evaluated once/at initialization.");
            //}

            if (!IsDynamic && !IsStatic)
            {
                //if (Init || EvaluateOnce)
                //{
                //    Modifiers.Add(FLKeywords.StaticElementModifier);
                //}
                //else
                //{
                Modifiers.Add(FLKeywords.DynamicElementModifier);
                //}
            }

            if (ComputeOnce && !IsStatic)
            {
                throw new FLInvalidFLElementModifierUseException(ElementName, "static/once",
                    $"The function {ElementName} can not have the modifier once unless static is set as well.");
            }

            if (!NoJump && IsStatic)
            {
                Modifiers.Add(FLKeywords.NoJumpKeyword);
                //throw new FLInvalidFLElementModifierUseException(ElementName, "nojump/static", "Can not use the static modifier without adding the nojump modifier.");
            }
        }
    }
}