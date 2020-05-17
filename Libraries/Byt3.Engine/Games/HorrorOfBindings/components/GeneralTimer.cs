using System;
using Byt3.Engine.Core;

namespace HorrorOfBindings.components
{
    public class GeneralTimer : AbstractComponent
    {
        private readonly Action _action;
        private readonly float _fireTime;
        private readonly bool _loop;
        private float _time;

        public GeneralTimer(float fireTime, Action action, bool loop = false)
        {
            _action = action;
            _fireTime = fireTime;
            _loop = loop;
        }


        protected override void Update(float deltaTime)
        {
            _time += deltaTime;
            if (_time >= _fireTime)
            {
                _action?.Invoke();
                if (_loop)
                {
                    _time = 0;
                }
                else
                {
                    Destroy();
                }
            }
        }
    }
}