using System.Collections.Generic;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters.enemies {
    [RegisteredType(nameof(EnemyCharacter), "", nameof(Resource)), GlobalClass]
    public partial class EnemyCharacter : Character {
        private enum Species {
            Human, Undead, Spirit, Beast, Humanoid
        }

        [Export] private Species Type { set; get; }

        private EnemyCharacter(
            string name, Species type, Texture2D avatar,
            IDictionary<Stat.Category, Stat> stats
        ) : base(name, avatar, stats) {
            this.Type = type;
        }
    }
}