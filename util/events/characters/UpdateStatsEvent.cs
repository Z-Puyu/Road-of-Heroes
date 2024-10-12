using System;
using System.Collections.Generic;
using Game.common.stats;

namespace Game.util.events.characters {
    public class UpdateStatsEvent : EventArgs {
        public Dictionary<StatType, int> Changes { get; private init; }

        public UpdateStatsEvent(Dictionary<StatType, int> changes) {
            this.Changes = changes;
        }
    }
}