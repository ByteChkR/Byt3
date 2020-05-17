using Byt3.ADL;
using Byt3.Engine.Debug;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace Byt3.Engine.Audio
{
    /// <summary>
    /// Static Class that contains the OpenAL Initialization code and some useful properties to query information to the OpenAL Api
    /// </summary>
    public static class AudioManager
    {
        private static readonly ADLLogger<DebugChannel> Logger =
            new ADLLogger<DebugChannel>(EngineDebugConfig.Settings, "AudioManager");

        /// <summary>
        /// Private field for the audio context
        /// </summary>
        private static AudioContext _context;

        /// <summary>
        /// Useful Property to check if something has gone wrong with OpenAL
        /// </summary>
        public static AlcError GetCurrentALcError => _context?.CurrentError ?? AlcError.InvalidContext;

        /// <summary>
        /// Initializes the OpenAL Audio Context
        /// </summary>
        public static void Initialize()
        {
            Logger.Log(DebugChannel.EngineAudio | DebugChannel.Log, "Initializing OpenAL", 10);
            _context = new AudioContext();
        }
    }
}