using System;
using Game.common.stats;
using Game.util.interfaces;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Character), "", nameof(Resource)), GlobalClass]
    public abstract partial class Character : Resource {     
        [Export] public Array<CharacterConsumableStat> CharacterConsumableStats { get; set; } = 
                CharacterConsumableStat.DEFAULT;
        [Export] public Array<CharacterAbilityStat> CharacterAbilityStats { get; set; } = 
                CharacterAbilityStat.DEFAULT;
        [Export] public Array<CharacterResistanceStat> CharacterResistanceStats { get; set; } = 
                CharacterResistanceStat.DEFAULT;
        [Export] public string Name { set; get; } = "";
        [Export] public Texture2D Avatar { set; get; } = null;

        public Character() {}

        public Character(string name, Texture2D avatar) {
            this.Name = name;
            this.Avatar = avatar;
        }
    }
}