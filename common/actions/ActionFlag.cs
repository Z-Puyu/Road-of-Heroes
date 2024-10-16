using System;

namespace Game.common.actions {
    [Flags]
    public enum ActionFlag {
        None = 0,
        Hit = 1,
        Critical = 2
    }
}