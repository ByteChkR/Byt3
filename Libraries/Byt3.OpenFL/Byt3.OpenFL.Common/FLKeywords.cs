namespace Byt3.OpenFL.Common
{
    public class FLKeywords
    {
        public static string InputBufferKey = "in";
        public static string ActiveBufferKey = "current";
        public static string EntryFunctionKey = "Main";


        public static string DefineKey = "--define ";
        public static string ScriptKey = "script";
        public static string TextureKey = "texture";
        public static string ArrayKey = "array";
        public static string CommentBeginKey = "#";
        public static string SetParserOptionKey = "--set";


        public static string StaticElementModifier = "static";
        public static string DynamicElementModifier = "dynamic";


        public static string ReadOnlyBufferModifier = "readonly";
        public static string ReadWriteBufferModifier = "readwrite";

        public static string NoJumpKeyword = "nojump";
        public static string NoCallKeyword = "nocall";
        public static string ComputeOnceKeyword = "once";
        public static string InitializeOnStartKey = "init";


        //ExtPP Include Plugin
        public static string IncludeInlineKeyword = "~includeinl";
        public static string IncludeKeyword = "~include";

        //ExtPP Warning/Error Plugin
        public static string PPWarningKeyword = "~warning";
        public static string PPErrorKeyword = "~error";

        //ExtPP Conditional Plugin
        public static string StartCondition = "~if";
        public static string ElseIfCondition = "~elseif";
        public static string ElseCondition = "~else";
        public static string EndCondition = "~endif";
        public static string DefineKeyword = "~define";
        public static string UndefineKeyword = "~undefine";
        public static string DefineScriptKey => DefineKey + ScriptKey;
        public static string DefineTextureKey => DefineKey + TextureKey;
        public static string DefineArrayKey => DefineKey + ArrayKey;


        public static string[] ProtectedKeywords => new[]
        {
            InputBufferKey, ActiveBufferKey, EntryFunctionKey, //in / current / Main
            SetParserOptionKey,
            IncludeInlineKeyword, IncludeKeyword, //PP Include Plugin
            PPWarningKeyword, PPErrorKeyword, //PP Warning Plugin
            StartCondition, ElseIfCondition, ElseCondition, EndCondition, //PP Conditional Plugin
            DefineKeyword, UndefineKeyword //PP Conditional Plugin
        };
    }
}