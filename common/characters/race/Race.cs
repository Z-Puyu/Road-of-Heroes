using System.Collections.Generic;
using Game.common.characters.skills;
using Game.util;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters.race {
    [RegisteredType(nameof(Race), "", nameof(Resource)), GlobalClass]
    public partial class Race : Resource {
        public enum Name {
            Human,
            Elf,
            Dwarf,
            Orc
        }

        [Export] public Name RaceName { set; get; }
        [Export] public string Description { set; get; }
        [Export] public Array<Skill> Skills { set; get; } = [];

        public void AdjustStats(IDictionary<Stat.Category, Stat> stats) {
            switch (this.RaceName) {
                case Name.Elf:
                    if (stats.TryGetValue(Stat.Category.Magicka, out Stat magicka1)) {
                        int offset = Utilities.Randi(10, 30);
                        stats[Stat.Category.Magicka] = magicka1 + (offset, offset, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.Agility, out Stat agility1)) {
                        stats[Stat.Category.Agility] = agility1 + (Utilities.Randi(0, 5), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.BleedResist, out Stat resist1)) {
                        stats[Stat.Category.BleedResist] = resist1 - (Utilities.Randi(5, 10), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.PoisonResist, out Stat resist2)) {
                        stats[Stat.Category.PoisonResist] = resist2 - (Utilities.Randi(5, 10), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.BurnResist, out Stat resist3)) {
                        stats[Stat.Category.BurnResist] = resist3 - (Utilities.Randi(15, 25), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.BlightResist, out Stat resist4)) {
                        stats[Stat.Category.BlightResist] = resist4 + (Utilities.Randi(5, 10), 0, 0);
                    }
                    break;
                case Name.Dwarf:
                    if (stats.TryGetValue(Stat.Category.Strength, out Stat strength1)) {
                        stats[Stat.Category.Strength] = strength1 + (Utilities.Randi(2, 5), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.Perception, out Stat perception)) {
                        stats[Stat.Category.Perception] = perception - (Utilities.Randi(1, 5), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.BurnResist, out Stat resist5)) {
                        stats[Stat.Category.BurnResist] = resist5 + (Utilities.Randi(10, 20), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.StunResist, out Stat resist6)) {
                        stats[Stat.Category.StunResist] = resist6 - (Utilities.Randi(10, 20), 0, 0);
                    }
                    break;
                case Name.Orc:
                    if (stats.TryGetValue(Stat.Category.Strength, out Stat strength2)) {
                        stats[Stat.Category.Strength] = strength2 + (Utilities.Randi(1, 3), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.Agility, out Stat agility2)) {
                        stats[Stat.Category.Agility] = agility2 - (0, Utilities.Randi(1, 5), 0);
                    }
                    if (stats.TryGetValue(Stat.Category.Magicka, out Stat magicka3)) {
                        int offset = Utilities.Randi(10, 20);
                        stats[Stat.Category.Magicka] = magicka3 - (offset, offset, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.BleedResist, out Stat resist7)) {
                        stats[Stat.Category.BleedResist] = resist7 + (Utilities.Randi(5, 15), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.PoisonResist, out Stat resist8)) {
                        stats[Stat.Category.PoisonResist] = resist8 + (Utilities.Randi(5, 15), 0, 0);
                    }
                    break;
                case Name.Human:
                    if (stats.TryGetValue(Stat.Category.BleedResist, out Stat resist9)) {
                        stats[Stat.Category.BleedResist] = resist9 + (Utilities.Randi(5, 10), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.PoisonResist, out Stat resist10)) {
                        stats[Stat.Category.PoisonResist] = resist10 + (Utilities.Randi(5, 10), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.StunResist, out Stat resist11)) {
                        stats[Stat.Category.StunResist] = resist11 + (Utilities.Randi(5, 15), 0, 0);
                    }
                    if (stats.TryGetValue(Stat.Category.BurnResist, out Stat resist12)) {
                        stats[Stat.Category.BurnResist] = resist12 - (Utilities.Randi(5, 20), 0, 0);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}