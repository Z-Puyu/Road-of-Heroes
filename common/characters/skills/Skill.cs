using System.Collections.Generic;
using Game.common.effects;
using Game.util;
using Godot;
using Godot.Collections;

namespace Game.common.characters.skills {
    [GlobalClass]
    public partial class Skill : Resource {
        [Export] private string Name { set; get; }
        [Export] private Texture2D Icon { set; get; }
        [Export] private int Precision { set; get; }
        [Export] private Vector2I UserPosition { set; get; }
        [Export] private Vector2I TargetPosition { set; get; }
        [Export] private Vector2I DamageMultiplier { set; get; }
        [Export] private Godot.Collections.Dictionary<Stat.Category, int> Cost { set; get; } = [];
        [Export] private Array<Effect> EffectsOnSelf { set; get; } = [];
        [Export] private Array<Effect> EffectsOnTarget { set; get; } = [];

        public void Fire(Character src, IEnumerable<Character> targets) {
            foreach (Stat.Category stat in this.Cost.Keys) {
                src.Update(stat, this.Cost[stat]);
            }
            foreach (Character target in targets) {
                int hitChance = src.Get(Stat.Category.Precision) + this.Precision - target.Get(Stat.Category.Agility);
                int dice = Utilities.Randi(1, 100);
                if (dice > hitChance) {
                    // Dodge!
                    continue;
                }
                int critChance = src.Get(Stat.Category.Perception);
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