using Game.common.characters;

namespace Game.util.events.combat {
    public class CombatActionEvent {
        public enum Type {
            PhysicalAttack,
            MagicalAttack,
            Healing
        }

        public Actor Source { get; protected init; }
        public Actor Target { get; protected init; }
        public Type Action { get; protected init; }
        public bool IsCritical { get; protected init; }

        private CombatActionEvent(Actor source, Actor target, Type action, bool isCritical) { 
            this.Source = source;
            this.Target = target;
            this.Action = action;
            this.IsCritical = isCritical;
        }

        private sealed class PhysicalAttackActionEvent : CombatActionEvent { 
            public PhysicalAttackActionEvent(
                Actor source, Actor target, bool isCritical
            ) : base(source, target, Type.PhysicalAttack, isCritical) {}
        }
    }
}