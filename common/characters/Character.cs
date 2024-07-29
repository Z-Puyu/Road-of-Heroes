using System;
using System.Collections.Generic;
using Game.common.characters.skills;
using Game.common.effects;
using Game.common.modifier;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.characters {
    [RegisteredType(nameof(Character), "", nameof(Resource)), GlobalClass]
    public abstract partial class Character : Resource {
        private readonly IDictionary<Stat.Category, Stat> stats;
        protected readonly string name;
        private readonly Texture2D avatar;
        protected readonly List<Skill> skills = [];
        private readonly ModifierManager modifier = new ModifierManager();

        public Texture2D Avatar => avatar;
        public string Name => name;
        public ModifierManager Modifier => modifier;

        protected Character(
            string name, Texture2D avatar, IDictionary<Stat.Category, Stat> stats
        ) {
            this.name = name;
            this.avatar = avatar;
            this.stats = stats;
        }

        public void AddModifier(Modifier modifier) {
            this.modifier.Collect(modifier);
        }

        public bool Get(Stat.Category stat, out Stat value) {
            if (this.stats.TryGetValue(stat, out value)) {
                return true;
            }
            return false;
        }
        
        public int Get(Stat.Category stat) {
            if (this.stats.TryGetValue(stat, out Stat value)) {
                return Math.Clamp(
                    this.modifier.Modify(value.Type, value.Value), value.MinValue, value.MaxValue
                );
            }
            return 0;
        }

        public void Update(Stat.Category stat, int offset, int maxOffset = 0, int minOffset = 0) {
            if (this.stats.TryGetValue(stat, out Stat value)) {
                this.stats[stat] = value + (offset, maxOffset, minOffset);
            }
        }
    }
}