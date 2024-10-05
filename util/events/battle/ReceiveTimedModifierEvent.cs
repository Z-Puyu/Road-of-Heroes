using Game.common.characters;
using Game.common.effects.eot;
using Godot.Collections;

namespace Game.util.events.battle {
    public class ReceiveTimedModifierEvent : ConditionalEvent<Actor> {
        private readonly Array<MoT> buffs;
        private readonly Array<MoT> debuffs;

        public ReceiveTimedModifierEvent(
            Actor target, Array<MoT> buffs, Array<MoT> debuffs
        ) : base(null, [target]) {
            this.buffs = buffs;
            this.debuffs = debuffs;
        }

        public Array<MoT> Buffs => buffs;
        public Array<MoT> Debuffs => debuffs;
    }
}