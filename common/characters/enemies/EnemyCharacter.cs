using System.Collections.Generic;
using Game.common.characters.skills;
using Game.common.modifier;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters.enemies {
    [RegisteredType(nameof(EnemyCharacter), "", nameof(Resource)), GlobalClass]
    public partial class EnemyCharacter : Character {
        [Export] private Species Type { set; get; }
        [Export] private Array<Stat> ListOfStats { set; get; } = [];
        [Export] private Array<Skill> ActiveSkills { set; get; } = [];

        public EnemyCharacter() : base() {}

        private EnemyCharacter(
            string name, Species type, Texture2D avatar,
            IDictionary<Stat.Category, Stat> stats
        ) : base(name, avatar, stats) {
            this.Type = type;
        }

        public void InitStats() {
            foreach (Stat stat in this.ListOfStats) {
                this.stats[stat.Type] = stat;
            }
            foreach (EffectModifier modifier in this.Type.Abilities) {
                this.Modifier.Collect(modifier);
            }
        }
    }
}