using System;
using System.Collections.Generic;

namespace Geostorm.Core
{
    public interface IGameEventListener
    {
        public void HandleEvents(in List<GameEvent> events);
    }
}
