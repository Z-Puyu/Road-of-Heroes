using System.Collections.Generic;
using Game.common.effects;
using Game.util;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters.skills {
    [RegisteredType(nameof(Skill), "", nameof(Resource)), GlobalClass]
    public partial class Skill : Resource {
        private enum Range { SingleEnemy, SingleAlly, AOEEnemy, AOEAlly, SelfOnly }

        [Export] private string Name { set; get; }
        [Export] private Texture2D Icon { set; get; }
        [Export] private bool NeverMiss { set; get; }
        [Export] private int Precision { set; get; }
        [Export] private Range TargetRange { set; get; }
        [Export] private Vector2I UserPosition { set; get; }
        [Export] private Vector2I TargetPosition { set; get; }
        [Export] private Vector2I DamageMultiplier { set; get; }
        [Export] private Godot.Collections.Dictionary<Stat.Category, int> Cost { set; get; } = [];
        [Export] private Array<Effect> EffectsOnSelf { set; get; } = [];
        [Export] private Array<Effect> EffectsOnTarget { set; get; } = [];
        [Export] private int UsageLimit { set; get; } = -1;

        public void Fire(CharacterCard src, IEnumerable<CharacterCard> targets) {
            foreach (Stat.Category stat in this.Cost.Keys) {
                src.Character.Update(stat, this.Cost[stat]);
            }
            foreach (CharacterCard target in targets) {
                int dice;
                if (!this.NeverMiss) {
                    int hitChance = src.Character.Get(Stat.Category.Precision) + this.Precision - 
                            target.Character.Get(Stat.Category.Agility);
                    dice = Utilities.Randi(1, 100);
                    if (dice > hitChance) {
                        // Dodge!
                        continue;
                    }
                }
                int critChance = src.Character.Get(Stat.Category.Perception);
                dice = Utilities.Randi(1, 100);
                foreach (Effect effect in this.EffectsOnTarget) {
                    effect.Apply(src, target, crit: dice <= critChance);
                }
            }
            foreach (Effect effect in this.EffectsOnSelf) {
                effect.Apply(src, src);
            }
        }     
    }
}