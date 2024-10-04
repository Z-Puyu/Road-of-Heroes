using System;
using System.Collections.Generic;
using Game.common.modifier;
using Game.common.stats;
using Game.util;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Character), "", nameof(Resource)), GlobalClass]
    public abstract partial class Character : Resource {     
        [Export] public Array<Stat> Stats { get; set; }
        [Export] public string Name { set; get; }
        [Export] public Texture2D Avatar { set; get; }
        //protected readonly Dictionary<Skill, int> skills = [];
        private int speedOffset = 0;
        //public Dictionary<Skill, int> Skills => skills;

        protected Character() {}

        protected Character(
            string name, Texture2D avatar
        ) {
            this.Name = name;
            this.Avatar = avatar;
        }

        public void Log() {
            Console.WriteLine($"{this.Name}:");
            foreach (Stat stat in this.Stats) {
                Console.WriteLine(stat);
            }
        }
    }
}