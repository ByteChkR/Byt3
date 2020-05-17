using System;
using Byt3.Engine.Exceptions;

namespace HorrorOfBindings.exceptions
{
    public class GameException : EngineException
    {
        public GameException(string errorMessage, Exception inner) : base(errorMessage, inner)
        {
        }

        public GameException(string errorMessage) : base(errorMessage)
        {
        }
    }
}