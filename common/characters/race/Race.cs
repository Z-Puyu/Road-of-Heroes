using System.Collections.Generic;
using Game.common.stats;
using Game.util.math;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters.race {
    [RegisteredType(nameof(Race), "", nameof(Resource)), GlobalClass]
    public partial class Race : Resource {
        public enum Species { Human, Elf, Dwarf, Orc }

        [Export] public Species Name { set; get; }
        [Export] public string Description { set; get; } = "";
        [Export(PropertyHint.Range, "1,30")] private int MinHP { set; get; } = 15;
        [Export(PropertyHint.Range, "1,40")] private int MaxHP { set; get; } = 25;
        [Export(PropertyHint.Range, "0,30")] private int MinMagicka { set; get; } = 10;
        [Export(PropertyHint.Range, "0,50")] private int MaxMagicka { set; get; } = 25;
        [Export] private Array<StatRange> BaseStatsRanges { set; get; } = [
            new StatRange(StatType.Agility),
            new StatRange(StatType.Speed),
            new StatRange(StatType.Strength),
            new StatRange(StatType.Perception),
            new StatRange(StatType.Precision),
            new StatRange(StatType.BleedResist),
            new StatRange(StatType.PoisonResist),
            new StatRange(StatType.BurnResist),
            new StatRange(StatType.BlightResist),
            new StatRange(StatType.StunResist)
        ];

        public Race() {}

        public List<Stat> InitRandomStats() {
            List<Stat> stats = [
                new CharacterStat(
                    StatType.Health, MathUtil.Randi(this.MinHP, this.MaxHP)
                ),
                new CharacterStat(
                    StatType.Magicka, MathUtil.Randi(this.MinMagicka, this.MaxMagicka)
                ),
                new CharacterStat(StatType.Sanity, 100),
                new CharacterStat(StatType.Stamina, 100)
            ];
            foreach (StatRange range in this.BaseStatsRanges) {
                stats.Add(
                    new CharacterAttribute(range.Type, MathUtil.Randi(range.Min, range.Max))
                );
            }
            return stats;
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

        public override string ToString() {
            return this.Name.ToString();
        }
    }
}