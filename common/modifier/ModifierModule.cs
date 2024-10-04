using System;
using System.Collections.Generic;
using Game.common.stats;
using Godot;

namespace Game.common.modifier {
    [GlobalClass]
    public partial class ModifierModule : Node {
        private readonly Dictionary<StatType, HashSet<Modifier>> modifiers = [];      
        private readonly Dictionary<int, Action> onExpire = [];
        private int time = 0;

        public Stat Modify(Stat stat) {
            (int, int, int) offset = (0, 0, 0);
            (int, int, int) multiplier = (100, 100, 100);
            if (this.modifiers.TryGetValue(stat.Type, out HashSet<Modifier> modifiers)) {
                foreach (Modifier modifier in modifiers) {
                    (int, int, int) triplet = modifier.ToTriplet();
                    if (modifier.UsePercentage) {
                        multiplier.Item1 += triplet.Item1;
                        multiplier.Item2 += triplet.Item2;
                        multiplier.Item3 += triplet.Item3;
                    } else {
                        offset.Item1 += triplet.Item1;
                        offset.Item2 += triplet.Item2;
                        offset.Item3 += triplet.Item3;
                    }
                }
            }
            multiplier.Item1 = Math.Max(multiplier.Item1, 0);
            multiplier.Item2 = Math.Max(multiplier.Item2, 0);
            multiplier.Item3 = Math.Max(multiplier.Item3, 0);
            return (stat + offset) * multiplier;
        }

        private void Remove(Modifier modifier) {
            if (modifier != null && this.modifiers.TryGetValue(modifier.TargetStat, out HashSet<Modifier> modifiers)) {
                if (modifiers.Remove(modifier) && modifiers.Count == 0) {
                    this.modifiers.Remove(modifier.TargetStat);
                    if (this.modifiers.Count == 0) {
                        this.time = 0;
                    }
                }
            } 
        }

        public void Collect(Modifier modifier) {
            int expireTime = this.time + modifier.TimeToLast;
            if (this.onExpire.TryGetValue(expireTime, out Action action)) {
                action += () => this.Remove(modifier);
            } else {
                this.onExpire.Add(expireTime, () => this.Remove(modifier));
            }
        }

        public void Clear() {
            this.modifiers.Clear();
            this.onExpire.Clear();
            this.time = 0;
        }

        public override string ToString() {
            List<string> lines = [];
            foreach (KeyValuePair<StatType, HashSet<Modifier>> pair in this.modifiers) {
                (int, int, int) offset = (0, 0, 0);
                (int, int, int) multiplier = (0, 0, 0);
                foreach (Modifier modifier in pair.Value) {
                    (int, int, int) triplet = modifier.ToTriplet();
                    if (modifier.UsePercentage) {
                        multiplier.Item1 += triplet.Item1;
                        multiplier.Item2 += triplet.Item2;
                        multiplier.Item3 += triplet.Item3;
                    } else {
                        offset.Item1 += triplet.Item1;
                        offset.Item2 += triplet.Item2;
                        offset.Item3 += triplet.Item3;
                    }
                }
                if (offset.Item1 != 0) {
                    lines.Add($"{pair.Key} {(offset.Item1 > 0 ? "+" : "")}{offset.Item1}");
                }
                if (offset.Item2 != 0) {
                    lines.Add($"Max {pair.Key} {(offset.Item2 > 0 ? "+" : "")}{offset.Item2}");
                }
                if (offset.Item3 != 0) {
                    lines.Add($"Min {pair.Key} {(offset.Item3 > 0 ? "+" : "")}{offset.Item3}");
                }
                if (multiplier.Item1 != 0) {
                    lines.Add($"{pair.Key} {(multiplier.Item1 > 0 ? "+" : "")}{Math.Clamp(multiplier.Item1, -100, 100)}");
                }
                if (multiplier.Item2 != 0) {
                    lines.Add($"Max {pair.Key} {(multiplier.Item1 > 0 ? "+" : "")}{Math.Clamp(multiplier.Item1, -100, 100)}");
                }
                if (multiplier.Item1 != 0) {
                    lines.Add($"Min {pair.Key} {(multiplier.Item1 > 0 ? "+" : "")}{Math.Clamp(multiplier.Item1, -100, 100)}");
                }
            }
            return string.Join('\n', lines);
        }
    }
}