using System;
using System.Collections.Generic;
using Game.common.characters.skills;
using Game.common.modifier;
using Game.util;
using Game.util.events.characters;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Character), "", nameof(Resource)), GlobalClass]
    public abstract partial class Character : Resource {
        protected readonly IDictionary<Stat.Category, Stat> stats = new 
                Dictionary<Stat.Category, Stat>();
        [Export] public string Name { set; get; }
        [Export] public Texture2D Avatar { set; get; }
        protected readonly Dictionary<Skill, int> skills = [];
        private readonly ModifierManager modifier = new ModifierManager();
        private int speedOffset = 0;

        public ModifierManager Modifier => modifier;
        public Dictionary<Skill, int> Skills => skills;

        protected Character() {}

        protected Character(
            string name, Texture2D avatar, IDictionary<Stat.Category, Stat> stats
        ) {
            this.Name = name;
            this.Avatar = avatar;
            this.stats = stats;
        }

        public void AddModifier(Modifier modifier) {
            this.modifier.Collect(modifier);
        }

        public void RandomiseSped() {
            this.speedOffset = Utilities.Randi(-3, 3);
        }

        public void ResetSpeed() {
            this.speedOffset = 0;
        }

        public bool Get(Stat.Category stat, out Stat value) {
            if (this.stats.TryGetValue(stat, out value)) {
                if (stat == Stat.Category.Speed) {
                    value += (this.speedOffset, 0, 0);
                }
                return true;
            }
            return false;
        }
        
        public int Get(Stat.Category stat) {
            if (this.stats.TryGetValue(stat, out Stat value)) {
                int val = this.modifier.Modify(value.Type, value.Value) 
                        + (stat == Stat.Category.Speed ? this.speedOffset : 0);
                return Math.Clamp(val, value.MinValue, value.MaxValue);
            }
            return 0;
        }

        public void Update(Stat.Category stat, int offset, int maxOffset = 0, int minOffset = 0) {
            if (this.stats.TryGetValue(stat, out Stat value)) {
                this.stats[stat] = value + (offset, maxOffset, minOffset);
                this.Publish(new StatValueUpdatedEvent(stat, value.Value, value.MaxValue));
            }
        }
    }
}