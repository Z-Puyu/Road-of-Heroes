using System;
using System.Collections.Generic;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.modifier {
    [RegisteredType(nameof(AggregateModifier), "", nameof(Resource)), GlobalClass]
    public partial class AggregateModifier : Resource {
        private int offset = 0;
        private int factor = 0;
        private readonly HashSet<Modifier> underlyingModifiers = [];
        private Action onRemove;

        public int Offset => offset;
        public int Factor => factor;

        public AggregateModifier() {}

        public AggregateModifier(int offset, int factor, Action onRemove) {
            this.offset = offset;
            this.factor = factor;
            this.onRemove = onRemove;
        }

        public static AggregateModifier Of(Modifier modifier, Action onRemove) {
            if (modifier == null) {
                throw new ArgumentException("Cannot initiate AggregateModifier with null!");
            }
            AggregateModifier mod; 
            if (modifier.UsePercentage) {
                mod = new AggregateModifier(0, modifier.Value, onRemove);
            } else {
                mod = new AggregateModifier(modifier.Value, 0, onRemove);
            }
            mod.underlyingModifiers.Add(modifier);
            return mod;
        }

        public void Collect(Modifier mod) {
            if (mod != null) {
                this.underlyingModifiers.Add(mod);
                mod.OnExpire = () => this.underlyingModifiers.Remove(mod);
                if (mod.UsePercentage) {
                    this.factor = Math.Max(-100, this.factor + mod.Value);
                    mod.OnExpire += () => this.factor -= mod.Value;
                } else {
                    this.offset += mod.Value;
                    mod.OnExpire -= () => this.offset -= mod.Value;
                }
                mod.OnExpire += () => {
                    if (this.offset == 0 && this.factor == 0) {
                        this.onRemove();
                    }
                };
            }
        }

        public int Modify(int @base) {
            return (int)Math.Round((@base + offset) * (1 + factor / 100.0));
        }

        public string Describe(string prefix) {
            if (this.offset != 0) {
                if (this.factor != 0) {
                    return $"{prefix} {(this.offset > 0 ? "+" : "")}{this.offset}\n" + 
                           $"{prefix} {(this.factor > 0 ? "+" : "")}{this.factor}%";
                }
                return $"{prefix} {(this.offset > 0 ? "+" : "")}{this.offset}\n";
            }
            if (this.factor != 0) {
                return $"{prefix} {(this.factor > 0 ? "+" : "")}{this.factor}%";
            }
            return prefix;
        }
    }
}