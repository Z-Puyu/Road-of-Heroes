using System;
using Game.common.stats;
using Game.util.interfaces;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Character), "", nameof(Resource)), GlobalClass]
    public abstract partial class Character : Resource, ILoggable {     
        [Export] public Array<Stat> Stats { get; set; } = [];
        [Export] public string Name { set; get; } = "";
        [Export] public Texture2D Avatar { set; get; } = null;
        private int speedOffset = 0;

        public Character() {}

        public Character(string name, Texture2D avatar) {
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