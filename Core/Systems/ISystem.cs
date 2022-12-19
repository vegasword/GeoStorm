using System;
using System.Collections.Generic;


namespace Geostorm.Core
{
    interface ISystem
    {
        public void Update(in GameInputs inputs, in GameData data, ref List<GameEvent> events);
    }
}
