using System.Threading.Tasks;
using Game.common.actions;
using Game.common.actions.combat;
using Game.common.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(CureAction), "", nameof(Resource)), GlobalClass]
    public partial class CureAction : CombatAction {
        [Export] private CombatEffect.Type TargetEffect { set; get; }

        public override Task Apply(Actor src, Actor target, ActionFlag flag = ActionFlag.None) {
            target.Cure(this.TargetEffect);
            return Task.CompletedTask;
        }

        public override string Describe(Actor src, Actor target)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            return $"Cure {this.TargetEffect}";
        }
    }
}