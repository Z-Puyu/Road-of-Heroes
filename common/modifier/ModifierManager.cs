using System.Collections.Generic;
using Game.common.characters;
using Game.common.effects;
using Godot;

namespace Game.common.modifier {
    [GlobalClass]
    public partial class ModifierManager : Node {
        private readonly Dictionary<Stat.Category, AggregateModifier> statModifiers = [];
        private readonly Dictionary<Effect.Type, AggregateModifier> effectModifiersOnReceive = [];
        private readonly Dictionary<Effect.Type, AggregateModifier> effectModifiersOnEmit = [];

        public int ModifyOnReceive(Effect.Type e, int value) {
            if (this.effectModifiersOnReceive.TryGetValue(e, out AggregateModifier modifier)) {
                return modifier.Modify(value);
            }
            return value;
        }     

        public int ModifyOnEmit(Effect.Type e, int value) {
            if (this.effectModifiersOnEmit.TryGetValue(e, out AggregateModifier modifier)) {
                return modifier.Modify(value);
            }
            return value;
        }      

        public int Modify(Stat.Category stat, int value) {
            if (this.statModifiers.TryGetValue(stat, out AggregateModifier modifier)) {
                return modifier.Modify(value);
            }
            return value;
        }

        public void Collect(Modifier modifier) {
            if (modifier is StatModifier mod1) {
                if (this.statModifiers.TryGetValue(mod1.TargetStat, out AggregateModifier m)) {
                    m.Collect(modifier);
                } else {
                    this.statModifiers.Add(
                        mod1.TargetStat, 
                        AggregateModifier.Of(mod1, () => this.statModifiers.Remove(mod1.TargetStat))
                    );
                }
            } else if (modifier is EffectModifier mod2) {
                if (mod2.ActOnReceive) {
                    if (this.effectModifiersOnReceive
                            .TryGetValue(mod2.TargetEffect, out AggregateModifier m)) {
                        m.Collect(modifier);
                    } else {
                        this.effectModifiersOnReceive.Add(
                            mod2.TargetEffect, 
                            AggregateModifier.Of(
                                mod2, () => this.effectModifiersOnReceive.Remove(mod2.TargetEffect)
                            )
                        );
                    }
                } else {
                    if (this.effectModifiersOnEmit
                            .TryGetValue(mod2.TargetEffect, out AggregateModifier m)) {
                        m.Collect(modifier);
                    } else {
                        this.effectModifiersOnEmit.Add(
                            mod2.TargetEffect, 
                            AggregateModifier.Of(
                                mod2, () => this.effectModifiersOnEmit.Remove(mod2.TargetEffect)
                            )
                        );
                    }
                }
            }
        }

        public override string ToString() {
            List<string> lines = [];
            foreach (KeyValuePair<Stat.Category, AggregateModifier> pair in this.statModifiers) {
                lines.Add(pair.Value.Describe(pair.Key.ToString()));
            }
            foreach (KeyValuePair<Effect.Type, AggregateModifier> pair 
                    in this.effectModifiersOnReceive) {
                lines.Add(pair.Value.Describe(pair.Key.ToString()));
            }
            foreach (KeyValuePair<Effect.Type, AggregateModifier> pair 
                    in this.effectModifiersOnEmit) {
                lines.Add(pair.Value.Describe(pair.Key.ToString()));
            }
            return string.Join('\n', lines); 
        }
    }
}