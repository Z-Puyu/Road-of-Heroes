using System.Collections.Generic;
using Game.common.characters.skills;
using Game.util;
using Godot;
using Godot.Collections;

namespace Game.common.characters.race {
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
                    if (stats.TryGetValue(Stat.Category.Magicka, out Stat magicka)) {
                        stats[Stat.Category.Magicka] = magicka + Utilities.Randi(20, 50);
                    }
                    if (stats.TryGetValue(Stat.Category.Agility, out Stat agility1)) {
                        stats[Stat.Category.Agility] = agility1 + Utilities.Randi(0, 5);
                    }
                    if (stats.TryGetValue(Stat.Category.Health, out Stat health)) {
                        stats[Stat.Category.Health] = health - Utilities.Randi(10, 30);
                    }
                    break;
                case Name.Dwarf:
                    if (stats.TryGetValue(Stat.Category.Strength, out Stat strength1)) {
                        stats[Stat.Category.Strength] = strength1 + Utilities.Randi(2, 5);
                    }
                    if (stats.TryGetValue(Stat.Category.Perception, out Stat perception)) {
                        stats[Stat.Category.Perception] = perception - Utilities.Randi(1, 5);
                    }
                    if (stats.TryGetValue(Stat.Category.Speed, out Stat speed1)) {
                        stats[Stat.Category.Speed] = speed1 - Utilities.Randi(2, 5);
                    }
                    break;
                case Name.Orc:
                    if (stats.TryGetValue(Stat.Category.Strength, out Stat strength2)) {
                        stats[Stat.Category.Strength] = strength2 + Utilities.Randi(1, 3);
                    }
                    if (stats.TryGetValue(Stat.Category.Agility, out Stat agility2)) {
                        stats[Stat.Category.Agility] = agility2 - Utilities.Randi(1, 5);
                    }
                    if (stats.TryGetValue(Stat.Category.Speed, out Stat speed2)) {
                        stats[Stat.Category.Speed] = speed2 - Utilities.Randi(0, 2);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}