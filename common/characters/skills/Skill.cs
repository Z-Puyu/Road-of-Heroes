using System.Linq;
using Game.common.actions;
using Game.common.actions.combat;
using Game.common.stats;
using Game.util.events;
using Game.util.events.characters;
using Game.util.math;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace Game.common.characters.skills {
    [RegisteredType(nameof(Skill), "", nameof(Resource)), GlobalClass]
    public partial class Skill : Resource {
        public enum Range { SingleEnemy, SingleAlly, AOEEnemy, AOEAlly, SelfOnly, Neighbours }

        [Export] private string Name { set; get; }
        [Export] public Texture2D Icon { set; get; }
        [Export] private bool NeverMiss { set; get; }
        [Export] private int Precision { set; get; }
        [Export] public Range TargetRange { set; get; }
        [Export] public Array<bool> ValidPositions { set; get; } = [false, false, false, false];
        [Export] public Array<bool> ValidTargets { set; get; } = [false, false, false, false];
        [Export] private Array<SkillCondition> SkillConditions { set; get; } = [];
        [Export] private Array<Cost> Costs { set; get; } = [];
        [Export] private Array<CombatAction> EffectsOnSelf { set; get; } = [];
        [Export] private Array<CombatAction> EffectsOnTarget { set; get; } = [];
        [Export] private int UsageLimit { set; get; } = -1;
        [Export] public Array<ClassWeight> ClassWeights { set; get; } = [];

        public bool IsUsableBy(Actor actor) {
            return this.SkillConditions.All(cond => cond.Test(actor)) 
                && this.Costs.All(cost => cost.IsAffordable(actor));
        }

        public void Fire(Actor src, Actor[] targets, int level = 1) {
            if (!this.IsUsableBy(src)) {
                return;
            }
            foreach (Cost cost in this.Costs) {
                cost.ConsumeBy(src);
            }
            foreach (Actor target in targets) {
                ActionFlag flag = ActionFlag.None;
                if (!this.NeverMiss) {
                    // Hit chance = base precision + actor precision bonus - enemy agility
                    int hitChance = src.Get(StatType.Precision) + 
                                    this.Precision - target.Get(StatType.Agility);
                    if (MathUtil.Randi(1, 100) <= hitChance) {
                        flag |= ActionFlag.Hit;
                    }
                }
                if (MathUtil.Randi(1, 100) <= src.Get(StatType.Perception)) {
                    flag |= ActionFlag.Critical;
                }
                foreach (CombatAction action in this.EffectsOnTarget) {
                    action.Apply(src, target, flag);
                }
            }
            foreach (CombatAction action in this.EffectsOnSelf) {
                action.Apply(src, src);
            }
        }

        /* public string ToDesc(Character character) {
            string str = $"{this.Name}\n"
                       + $"{(this.UsageLimit > 0 ? $"\nMaximum Number of Use: {this.UsageLimit}\n" : "")}";
            if (this.SkillConditions.Count > 0) {
                str += "Can be used only if: \n";
                foreach (SkillCondition req in this.SkillConditions) {
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
        } */

        /* public override string ToString() {
            string str = $"{this.Name}\n"
                       + $"{(this.UsageLimit > 0 ? $"\nMaximum Number of Use: {this.UsageLimit}\n" : "")}";
            if (this.SkillConditions.Count > 0) {
                str += "Can be used only if: \n";
                foreach (SkillCondition req in this.SkillConditions) {
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
        } */
    }
}