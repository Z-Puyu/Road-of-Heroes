using Game.common.effects;
using Game.common.modifier;
using Godot;
using Godot.Collections;

namespace Game.common.characters {
    public partial class Character : Resource, IEffectEmitter, IEffectReceiver {
        private readonly Dictionary<Stat.Category, Stat> stats = [];
        private readonly string name;
        private int level;
        private readonly ModifierManager modifier = new ModifierManager();

        public Character(string name, int level, params Stat[] stats) {
            this.name = name;
            this.level = level;
            foreach (Stat stat in stats) {
                this.stats.Add(stat.Type, stat);
            }
        }
        
        public int Get(Stat.Category stat) {
            if (this.stats.TryGetValue(stat, out Stat value)) {
                return this.modifier.Modify(value.Type, value.Value);
            }
            return 0;
        }

        public int Emit(Effect.Type effect, int value) {
            return this.modifier.ModifyOnEmit(effect, value);
        }

        public int Receive(Effect.Type effect, int value) {
            return this.modifier.ModifyOnReceive(effect, value);
        }

        public override string ToString() {
            string proficiency = this.level switch {
                0 => "Novice",
                1 => "Amateur",
                2 => "Apprentice",
                3 => "Professional",
                4 => "Expert",
                5 => "Master",
                6 => "Legendary",
                _ => "",
            };
            return this.name;                                                                                                   
        }
    }
}