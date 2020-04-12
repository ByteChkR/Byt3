using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Byt3.ADL;
using Byt3.ExtPP.Base;
using Byt3.ExtPP.Base.Interfaces;
using Byt3.ExtPP.Base.Plugins;
using Byt3.ExtPP.Base.settings;
using Utils = Byt3.ExtPP.Base.Utils;

namespace Byt3.ExtPP.Plugins
{
    public class TextEncoderPlugin : AbstractFullScriptPlugin
    {
        public class TextEncoding
        {
            public delegate string EncodeDel(string text, string[] parameter);

            public delegate string DecodeDel(string text, string[] parameter);

            public string Key { get; private set; }
            private readonly EncodeDel encode;
            private readonly DecodeDel decode;

            public TextEncoding(string key, EncodeDel encode, DecodeDel decode)
            {
                Key = key;
                this.encode = encode;
                this.decode = decode;
            }

            public string Encode(string text, string[] parameter)
            {
                return encode?.Invoke(text, parameter);
            }

            public string Decode(string text, string[] parameter)
            {
                return decode?.Invoke(text, parameter);
            }

            #region En/Decodings

            public static TextEncoding Base64 { get; } = new TextEncoding("b64", Encode_BASE64, Decode_BASE64);
            public static TextEncoding Rot { get; } = new TextEncoding("rot",
                (text, parameter) => DeEncode_ROT(text, parameter, true),
                (text, parameter) => DeEncode_ROT(text, parameter, false));


            #region BASE64

            public static string Decode_BASE64(string text, string[] parameter)
            {
                return Encoding.Default.GetString(Convert.FromBase64String(text));
            }

            public static string Encode_BASE64(string text, string[] parameter)
            {
                return Convert.ToBase64String(Encoding.Default.GetBytes(text));
            }

            #endregion

            #region ROT

            private const char SPACE = ' ';
            private const char LOWER_A = 'a';
            private const char UPPER_A = 'A';

            public static string DeEncode_ROT(string text, string[] parameter, bool encode)
            {
                int amount = Math.Abs(int.Parse(parameter[0]) % 13);
                StringBuilder ret = new StringBuilder();
                for (int i = 0; i < text.Length; i++)
                {
                    int offset;
                    char begin;
                    if (text[i] == SPACE)
                    {
                        ret.Append(' ');
                        continue;

                    }

                    if (char.IsLower(text[i]))
                    {
                        offset = text[i] - LOWER_A;
                        begin = LOWER_A;
                    }
                    else
                    {
                        offset = text[i] - UPPER_A;
                        begin = UPPER_A;
                    }
                    if (encode)
                    {
                        offset += amount;
                        offset %= 26;
                    }
                    else
                    {
                        offset -= amount;
                        if (offset < 0)
                        {
                            offset = 26 + offset;
                        }
                    }
                    ret.Append((char) (begin + offset));
                }

                return ret.ToString();
            }

            #endregion

            #endregion
        }

        private static readonly List<TextEncoding> Encoders = new List<TextEncoding>
        {
            TextEncoding.Base64, TextEncoding.Rot
        };

        public override ProcessStage ProcessStages => Stage.ToLower(CultureInfo.InvariantCulture) == "onload"
            ? ProcessStage.OnLoadStage
            : ProcessStage.OnMain;

        public override string[] Cleanup => new[]
        {
            BlockEncodeStartKeyword, BlockEncodeEndKeyword, BlockDecodeStartKeyword, BlockDecodeEndKeyword
        };


        public string Stage { get; set; } = "onload";

        public string BlockEncodeStartKeyword { get; set; } = "#block encode";

        public string BlockEncodeEndKeyword { get; set; } = "#endblock encode";

        public string BlockDecodeStartKeyword { get; set; } = "#block decode";

        public string BlockDecodeEndKeyword { get; set; } = "#endblock decode";

        public override string[] Prefix => new[] {"tenc", "TextEncoderPlugin"};


        public override List<CommandInfo> Info { get; } = new List<CommandInfo>
        {
            new CommandInfo("set-stage", "ss", PropertyHelper.GetPropertyInfo(typeof(TextEncoderPlugin), nameof(Stage)),
                "Sets the Stage Type of the Plugin to be Executed OnLoad or OnMain"),
            new CommandInfo("set-start-encode-keyword", "ssek",
                PropertyHelper.GetPropertyInfo(typeof(TextEncoderPlugin), nameof(BlockEncodeStartKeyword)),
                "Sets the keyword that is used to open a Encode block"),
            new CommandInfo("set-end-encode-keyword", "seek",
                PropertyHelper.GetPropertyInfo(typeof(TextEncoderPlugin), nameof(BlockEncodeEndKeyword)),
                "Sets the keyword that is used to end a Encode block"),
            new CommandInfo("set-start-decode-keyword", "ssdk",
                PropertyHelper.GetPropertyInfo(typeof(TextEncoderPlugin), nameof(BlockDecodeStartKeyword)),
                "Sets the keyword that is used to open a Decode block"),
            new CommandInfo("set-end-decode-keyword", "sedk",
                PropertyHelper.GetPropertyInfo(typeof(TextEncoderPlugin), nameof(BlockDecodeEndKeyword)),
                "Sets the keyword that is used to end a Decode block"),
        };


        public override void Initialize(Settings settings, ISourceManager sourceManager, IDefinitions defs)
        {
            List<CommandInfo> info = Info;
            settings.ApplySettings(info, this);
        }


        private bool TryParseDecodeStatement(string line, out TextEncoding encoding, out string[] parameter)
        {


            string[] data = line.Replace(BlockDecodeStartKeyword, "").Replace(BlockEncodeStartKeyword, "").Trim()
                .Split(' ');

            parameter = new string[0];
            if (data.Length == 0 || !Encoders.TryFindByKey(data[0], out encoding))
            {
                Logger.Log(PPLogType.Error, Verbosity.Level1, "Decode block has no Specified decoding scheme.");

                encoding = null;
                parameter = null;
                return false;
            }

            if (data.Length > 1)
            {
                parameter = Utils.SplitAndRemoveFirst(data.Unpack(" "), " ");
            }


            return true;

        }

        public override bool FullScriptStage(ISourceScript file, ISourceManager todo, IDefinitions defs)
        {
            List<string> lines = file.GetSource().ToList();
            List<int> removeIndices = new List<int>();
            Logger.Log(PPLogType.Log, Verbosity.Level5, "Discovering Block Keywords.");
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].TrimStart();
                if (line.StartsWith(BlockEncodeStartKeyword))
                {

                    removeIndices.Add(i);

                    bool encodingOk = TryParseDecodeStatement(lines[i].TrimStart(), out TextEncoding enc,
                        out string[] encParameter);

                    i++; //Move forward.
                    if (!encodingOk)
                    {
                        Logger.Log(PPLogType.Error, Verbosity.Level1, "Could not load encoder: {0}", lines[i]);
                    }


                    Logger.Log(PPLogType.Log, Verbosity.Level6, "Found Block Encode Keyword.");
                    for (; i < lines.Count; i++)
                    {
                        if (lines[i].TrimStart().StartsWith(BlockEncodeEndKeyword))
                        {
                            removeIndices.Add(i);
                            i++; //Move Forward
                            break;
                        }

                        Logger.Log(PPLogType.Log, Verbosity.Level7, "Encoding line {0}.", i);
                        lines[i] = encodingOk ? enc.Encode(lines[i], encParameter) : lines[i];

                    }
                }
                else if (line.StartsWith(BlockDecodeStartKeyword))
                {
                    removeIndices.Add(i);

                    bool decodingOk = TryParseDecodeStatement(lines[i].TrimStart(), out TextEncoding enc,
                        out string[] encParameter);
                    i++; //Move forward.

                    if (!decodingOk)
                    {
                        Logger.Log(PPLogType.Error, Verbosity.Level1, "Could not load decoder: {0}", lines[i]);
                    }

                    Logger.Log(PPLogType.Log, Verbosity.Level6, "Found Block Decode Keyword.");
                    for (; i < lines.Count; i++)
                    {
                        if (lines[i].TrimStart().StartsWith(BlockDecodeEndKeyword))
                        {
                            removeIndices.Add(i);
                            i++; //Move Forward
                            break;
                        }

                        Logger.Log(PPLogType.Log, Verbosity.Level7, "Decoding line {0}.", i);
                        lines[i] = decodingOk ? enc.Decode(lines[i], encParameter) : lines[i];

                    }
                }


            }

            for (int i = removeIndices.Count - 1; i >= 0; i--)
            {
                lines.RemoveAt(removeIndices[i]);
            }
            removeIndices.Clear();


            file.SetSource(lines.ToArray());


            return true;
        }
    }
}