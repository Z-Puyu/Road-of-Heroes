using System.Collections.Generic;
using Game.common.stats;
using Game.util;
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
        [ExportGroup("Racial Variations in Base Stats")]
        [Export] public Array<CharacterAbilityStat> BaseAbilities { set; get; } = [];
        [Export] public Array<CharacterConsumableStat> BasePhysicalConditions { set; get; } = [];
        [Export] public Array<CharacterResistanceStat> BaseResistances { set; get; } = [];

        public Race() {}

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