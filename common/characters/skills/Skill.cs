using System;
using Game.common.characters.profession;
using Game.common.effects;
using Game.util;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters.skills {
    [RegisteredType(nameof(Skill), "", nameof(Resource)), GlobalClass]
    public partial class Skill : Resource {
        private enum Range { SingleEnemy, SingleAlly, AOEEnemy, AOEAlly, SelfOnly }

        [Export] public bool IsRacialSkill { set; get; } = false;
        [Export] private string Name { set; get; }
        [Export] public Texture2D Icon { set; get; }
        [Export] private bool NeverMiss { set; get; }
        [Export] private int Precision { set; get; }
        [Export] private Range TargetRange { set; get; }
        [Export] private Vector2I UserPosition { set; get; }
        [Export] private Vector2I TargetPosition { set; get; }
        [Export] private Array<Requirement> Requirements { set; get; } = [];
        [Export] private Array<Cost> Costs { set; get; } = [];
        [Export] private Array<Effect> EffectsOnSelf { set; get; } = [];
        [Export] private Array<Effect> EffectsOnTarget { set; get; } = [];
        [Export] private bool CanBeLearnt { set; get; } = true;
        [Export] private int UsageLimit { set; get; } = -1;
        [Export] public Dictionary<Profession, int> ProfessionScores { set; get; } = [];

        public bool IsUsableBy(Character character) {
            foreach (Requirement req in this.Requirements) {
                if (!req.Test(character)) {
                    return false;
                }
            }
            foreach (Cost cost in this.Costs) {
                if (character.Get(cost.StatType, out Stat value)) {
                    if (value.Value < cost.Compute(value.MaxValue)) {
                        return false;
                    }
                } 
            }
            return true;
        }

        public void Fire(CharacterCard src, CharacterCard[] targets, int level = 1) {
            if (!this.IsUsableBy(src.Character)) {
                return;
            }
            foreach (Cost cost in this.Costs) {
                if (src.Character.Get(cost.StatType, out Stat value)) {
                    src.Character.Update(cost.StatType, cost.Compute(value.MaxValue));
                } 
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

        public string ToDesc(Character character) {
            string str = $"{this.Name}\n"
                       + $"{(this.UsageLimit > 0 ? $"\nMaximum Number of Use: {this.UsageLimit}\n" : "")}";
            if (this.Requirements.Count > 0) {
                str += "Can be used only if: \n";
                foreach (Requirement req in this.Requirements) {
                    str += $"{req}\n";
                }
            }
            if (this.Costs.Count > 0) {
                str += "\nCost: \n";
                foreach (Cost cost in this.Costs) {
                    str += $"{cost}\n";
                }
            }
            int left = Math.Min(this.UserPosition.X, this.UserPosition.Y);
            int right = Math.Max(this.UserPosition.X, this.UserPosition.Y);
            str += $"\nCan be used at {(left == right ? $"position {left}" 
                                                    : $"positions {left} to {right}")}\n";
            left = Math.Min(this.TargetPosition.X, this.TargetPosition.Y);
            right = Math.Max(this.TargetPosition.X, this.TargetPosition.Y);
            str += $"Can target {(left == right ? $"position {left}"
                                                : $"positions {left} to {right}")}\n";
            str += $"\nBase precision: {this.Precision}";
            if (this.EffectsOnTarget.Count > 0) {
                str += this.TargetRange switch {
                    Range.SingleEnemy => $"\n\nEnemy target:",
                    Range.AOEEnemy => "\n\nEnemy target:",
                    Range.SingleAlly => "\n\nAlly target:",
                    Range.AOEAlly => "\n\nAlly target:",
                    Range.SelfOnly => "\n\nSelf:",
                    _ => ""
                };
                foreach (Effect e in this.EffectsOnTarget) {
                    str += $"\n{e.ToDesc(character)}";
                }
            }
            if (this.EffectsOnSelf.Count > 0) {
                str += $"\n\nSelf:";
                foreach (Effect e in this.EffectsOnSelf) {
                    str += $"\n{e.ToDesc(character)}";
                }
            }
            return str;
        }

        public override string ToString() {
            string str = $"{this.Name}\n"
                       + $"{(this.UsageLimit > 0 ? $"\nMaximum Number of Use: {this.UsageLimit}\n" : "")}";
            if (this.Requirements.Count > 0) {
                str += "Can be used only if: \n";
                foreach (Requirement req in this.Requirements) {
                    str += $"{req}\n";
                }
            }
            if (this.Costs.Count > 0) {
                str += "\nCost: \n";
                foreach (Cost cost in this.Costs) {
                    str += $"{cost}\n";
                }
            }
            int left = Math.Min(this.UserPosition.X, this.UserPosition.Y);
            int right = Math.Max(this.UserPosition.X, this.UserPosition.Y);
            str += $"\nCan be used at {(left == right ? $"position {left}" 
                                                    : $"positions {left} to {right}")}\n";
            left = Math.Min(this.TargetPosition.X, this.TargetPosition.Y);
            right = Math.Max(this.TargetPosition.X, this.TargetPosition.Y);
            str += $"Can target {(left == right ? $"position {left}"
                                                : $"positions {left} to {right}")}\n";
            str += $"\nBase precision: {this.Precision}";
            if (this.EffectsOnTarget.Count > 0) {
                str += this.TargetRange switch {
                    Range.SingleEnemy => $"\n\nEnemy target:",
                    Range.AOEEnemy => "\n\nEnemy target:",
                    Range.SingleAlly => "\n\nAlly target:",
                    Range.AOEAlly => "\n\nAlly target:",
                    Range.SelfOnly => "\n\nSelf:",
                    _ => ""
                };
                foreach (Effect e in this.EffectsOnTarget) {
                    str += $"\n{e}";
                }
            }
            if (this.EffectsOnSelf.Count > 0) {
                str += $"\n\nSelf:";
                foreach (Effect e in this.EffectsOnSelf) {
                    str += $"\n{e}";
                }
            }
            return str;
        }
    }
}