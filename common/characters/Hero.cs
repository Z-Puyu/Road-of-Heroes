using System;
using Game.common.autoload;
using Game.common.characters.classes;
using Game.common.characters.race;
using Game.common.stats;
using Game.util;
using Game.util.math;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Hero), "", nameof(Resource)), GlobalClass]
    public partial class Hero : Character {
        private enum Level {
            Novice, Amateur, Apprentice, Professional, Expert, Master, Legendary
        }

        [Export] private Level Proficiency { set; get; } = Level.Novice;
        [Export] private Race Race { set; get; }
        [Export] private Class Class { set; get; }

        public Hero() {}

        private Hero(string name, Level proficiency, Race race) : base(name, null) {
            this.Proficiency = proficiency;
            this.Race = race;
        }

        public static Hero Random(int level = 0) {
            Level proficiency = (Level)Math.Clamp(level, 0, (int)Level.Legendary);
            return new Hero("New Hero", proficiency, GameManager.RandomRace());
        }

        public override string ToString() {
            return $"{this.Proficiency} {this.Race} {this.Class} {this.Name}";
        }
    }
}