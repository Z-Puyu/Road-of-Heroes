using System.Threading.Tasks;
using Game.common.characters;
using Game.common.stats;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.effects {
    [RegisteredType(nameof(MagicAttackEffect), "", nameof(Resource)), GlobalClass]
    public partial class MagicAttackEffect : Effect<Actor, Actor> {
        [Export] private int MinDamage { set; get; }
        [Export] private int MaxDamage { set; get;}

        public override async Task Apply(Actor src, Actor target, bool crit = false) {
            src.Attack(StatType.MagicDamageDealt, this.MinDamage, this.MaxDamage, target, crit);
        }

        public override string ToString() {
            return this.MinDamage == this.MaxDamage
                    ? $"{this.MinDamage} magic damage" 
                    : $"{this.MinDamage} to {this.MaxDamage} magic damage";
        }

        public override string ToDesc(Actor actor) {
            return this.ToString();
        }
    }
}