using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace Game.common.characters {
    public partial class Stat : Resource {
        public enum Category {
            Health,
            Magicka,
            Sanity,
            Fatigue,
            Speed,
            Agility,
            Strength,
            Perception
        }

        [Export] public Category Type { get; set; }
        [Export] public int Value { set; get; }
    }
}