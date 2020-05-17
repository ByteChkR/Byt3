namespace Byt3.OpenFL.Common.ElementModifiers
{
    public class FLExecutableElementModifiers : FLElementModifiers
    {
        //protected virtual bool EvaluateOnce => Modifiers.Contains(FLKeywords.EvaluateOnceKeyword);
        //protected virtual bool Init => Modifiers.Contains(FLKeywords.InitOnStartKeyword);

        public FLExecutableElementModifiers(string functionName, string[] modifiers) : base(functionName, modifiers)
        {
        }

        protected override string[] ValidKeywords => new[]
            {FLKeywords.ScriptKey, FLKeywords.NoJumpKeyword, FLKeywords.NoCallKeyword, FLKeywords.ComputeOnceKeyword};

        public bool NoJump => Modifiers.Contains(FLKeywords.NoJumpKeyword);
        public bool NoCall => Modifiers.Contains(FLKeywords.NoCallKeyword);
        public bool ComputeOnce => Modifiers.Contains(FLKeywords.ComputeOnceKeyword);

        public virtual bool InitializeOnStart => Modifiers.Contains(FLKeywords.InitializeOnStartKey);

        protected override void Validate()
        {
            if (!ComputeOnce && InitializeOnStart)
            {
                Modifiers.Add(FLKeywords.ComputeOnceKeyword);
            }

            //if (Init && !EvaluateOnce)
            //{
            //    Modifiers.Add(FLKeywords.EvaluateOnceKeyword);
            //}
        }
    }
}