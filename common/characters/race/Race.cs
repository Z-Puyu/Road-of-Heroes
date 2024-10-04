using System.Collections.Generic;
using Game.common.stats;
using Game.util;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters.race {
    [RegisteredType(nameof(Race), "", nameof(Resource)), GlobalClass]
    public partial class Race : Resource {
        public enum Species { Human, Elf, Dwarf, Orc }

        [Export] public Species Name { set; get; }
        [Export] public string Description { set; get; } = "";
        [Export] public Array<Stat> RacialVariations { set; get; } = [];

        public Race() {}

        public void AdjustStats(IDictionary<StatType, Stat> stats) {
            switch (this.Name) {
                case Species.Elf:
                    if (stats.TryGetValue(StatType.Magicka, out Stat magicka1)) {
                        int offset = Utilities.Randi(10, 30);
                        stats[StatType.Magicka] = magicka1 + (offset, offset, 0);
                    }
                    if (stats.TryGetValue(StatType.Agility, out Stat agility1)) {
                        stats[StatType.Agility] = agility1 + (Utilities.Randi(0, 5), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.BleedResist, out Stat resist1)) {
                        stats[StatType.BleedResist] = resist1 - (Utilities.Randi(5, 10), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.PoisonResist, out Stat resist2)) {
                        stats[StatType.PoisonResist] = resist2 - (Utilities.Randi(5, 10), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.BurnResist, out Stat resist3)) {
                        stats[StatType.BurnResist] = resist3 - (Utilities.Randi(15, 25), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.BlightResist, out Stat resist4)) {
                        stats[StatType.BlightResist] = resist4 + (Utilities.Randi(5, 10), 0, 0);
                    }
                    break;
                case Species.Dwarf:
                    if (stats.TryGetValue(StatType.Strength, out Stat strength1)) {
                        stats[StatType.Strength] = strength1 + (Utilities.Randi(2, 5), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.Perception, out Stat perception)) {
                        stats[StatType.Perception] = perception - (Utilities.Randi(1, 5), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.BurnResist, out Stat resist5)) {
                        stats[StatType.BurnResist] = resist5 + (Utilities.Randi(10, 20), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.StunResist, out Stat resist6)) {
                        stats[StatType.StunResist] = resist6 - (Utilities.Randi(10, 20), 0, 0);
                    }
                    break;
                case Species.Orc:
                    if (stats.TryGetValue(StatType.Strength, out Stat strength2)) {
                        stats[StatType.Strength] = strength2 + (Utilities.Randi(1, 3), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.Agility, out Stat agility2)) {
                        stats[StatType.Agility] = agility2 - (0, Utilities.Randi(1, 5), 0);
                    }
                    if (stats.TryGetValue(StatType.Magicka, out Stat magicka3)) {
                        int offset = Utilities.Randi(10, 20);
                        stats[StatType.Magicka] = magicka3 - (offset, offset, 0);
                    }
                    if (stats.TryGetValue(StatType.BleedResist, out Stat resist7)) {
                        stats[StatType.BleedResist] = resist7 + (Utilities.Randi(5, 15), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.PoisonResist, out Stat resist8)) {
                        stats[StatType.PoisonResist] = resist8 + (Utilities.Randi(5, 15), 0, 0);
                    }
                    break;
                case Species.Human:
                    if (stats.TryGetValue(StatType.BleedResist, out Stat resist9)) {
                        stats[StatType.BleedResist] = resist9 + (Utilities.Randi(5, 10), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.PoisonResist, out Stat resist10)) {
                        stats[StatType.PoisonResist] = resist10 + (Utilities.Randi(5, 10), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.StunResist, out Stat resist11)) {
                        stats[StatType.StunResist] = resist11 + (Utilities.Randi(5, 15), 0, 0);
                    }
                    if (stats.TryGetValue(StatType.BurnResist, out Stat resist12)) {
                        stats[StatType.BurnResist] = resist12 - (Utilities.Randi(5, 20), 0, 0);
                    }
                    break;
                default:
                    break;
            }
        }

        public string ToAdj() {
            return this.Name switch {
                Species.Human => "Human",
                Species.Elf => "Elven",
                Species.Dwarf => "Dwarven",
                Species.Orc => "Orcish",
                _ => "",
            };
        }
    }
}