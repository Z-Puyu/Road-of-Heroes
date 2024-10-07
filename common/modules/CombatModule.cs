using System;
using Game.common.characters;
using Game.common.effects;
using Game.common.stats;
using Game.util;
using Game.util.events;
using Game.util.events.battle;
using Game.util.math;
using Godot;

namespace Game.common.modules {
    public partial class CombatModule : Node {
        private Actor Root { set; get; }

        public override async void _Ready() {
            await this.ToSignal(this.GetParent(), SignalName.Ready);
            this.Root = this.GetParent<Actor>();
            this.Subscribe<AttackEvent>(this.OnAttack);
            this.Subscribe<AttackedEvent>(this.OnAttacked);
            this.Subscribe<HealingEvent>(this.OnHeal);
            this.Subscribe<HealedEvent>(this.OnHealed);
        }

        private void OnHealed(HealedEvent e) {
            if (e.HandledBy(this.Root)) {
                ModifiableValueType healedModifiableValue = e.Heal.TargetModifiableValue;
                ModifiableValue heal = this.Root.Filter(e.Heal);
                this.Root.Update(healedModifiableValue, heal.Value);
            }
        }


        private void OnHeal(HealingEvent e) {
            if (e.HandledBy(this.Root)) {
                this.Publish(new HealedEvent(e.Target, CombatModule.GenerateHeal(
                    e.HealTarget, e.MinHeal, e.MaxHeal, e.IsCritical
                )));
            }
        }


        private void OnAttack(AttackEvent e) {
            if (e.HandledBy(this.Root)) {
                double min = e.MinDmg * e.DmgMult / 100.0;
                double max = e.MaxDmg * e.DmgMult / 100.0;
                ModifiableValue dmg = this.Root.Filter(CombatModule.GenerateDamage(
                    e.DmgType, min, max, e.IsCritical
                ));
                this.Publish(new AttackedEvent(e.Target, dmg));
            }
        }

        public void OnAttacked(AttackedEvent e) {
            if (e.HandledBy(this.Root)) {
                this.Root.Update(ModifiableValueType.Health, -this.Root.Filter(e.Dmg).Value);
            }
        }

        private static Damage GenerateDamage(ModifiableValueType dmgType, double min, double max, bool crit) {
            int amount = crit ? (int)Math.Ceiling(max * 1.5) 
                              : MathUtil.Randi((int)Math.Ceiling(min), (int)Math.Ceiling(max));
            return dmgType switch {
                ModifiableValueType.MeleeDamageDealt => new Damage(ModifiableValueType.MeleeDamageTaken, amount),
                ModifiableValueType.RangedDamageDealt => new Damage(ModifiableValueType.RangedDamageTaken, amount),
                ModifiableValueType.MagicDamageDealt => new Damage(ModifiableValueType.MagicDamageTaken, amount),
                _ => new Damage(ModifiableValueType.MeleeDamageTaken, amount)
            };
        }

        private static Heal GenerateHeal(ModifiableValueType targetModifiableValue, int min, int max, bool crit) {
            int amount = crit ? (int)Math.Ceiling(max * 1.5)
                              : MathUtil.Randi(min, max);
            return new Heal(targetModifiableValue, amount);
        }
    }
}