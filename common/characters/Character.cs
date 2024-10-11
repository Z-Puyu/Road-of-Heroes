using System;
using Game.common.stats;
using Game.util.interfaces;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Character), "", nameof(Resource)), GlobalClass]
    public abstract partial class Character : Resource, ILoggable {     
        [Export] public Array<CharacterAttribute> Attributes { get; private set; } = CharacterAttribute.DEFAULT;
        [Export] public Array<CharacterStat> Stats { get; protected set; } = CharacterStat.ENEMY_DEFAULT;
        [Export] public string Name { set; get; } = "";
        [Export] public Texture2D Avatar { set; get; } = null;
        private int speedOffset = 0;

        public Character() {}

        public Character(
            string name, Texture2D avatar, Array<CharacterStat> stats, 
            Array<CharacterAttribute> attributes
        ) {
            this.Name = name;
            this.Avatar = avatar;
            this.Stats = stats;
            this.Attributes = attributes;
        }

        public virtual void Log() {
            Console.WriteLine($"{this.Name} statistics:");
            foreach (CharacterStat stat in this.Stats) {
                Console.WriteLine(stat);
            }
            foreach (CharacterAttribute attribute in this.Attributes) {
                Console.WriteLine(attribute);
            }  
        }
    }
}