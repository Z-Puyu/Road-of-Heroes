using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(MagicAttackEffect), "", nameof(Resource)), GlobalClass]
    public partial class MagicAttackEffect : Effect<Actor, Actor> {
        [Export] private Damage MinDamage { set; get; }
        [Export] private Damage MaxDamage { set; get;}

        public override async Task Apply(Actor src, Actor target, bool crit = false) {
            src.Attack(this.MinDamage, this.MaxDamage, target, crit);
        }

        public override string ToString() {
            return this.MinDamage.Value == this.MaxDamage.Value
                    ? $"{this.MinDamage.Value} magic damage" 
                    : $"{this.MinDamage.Value} to {this.MaxDamage.Value} magic damage";
        }

        public override string ToDesc(Actor actor) {
            return this.ToString();
        }
    }
}