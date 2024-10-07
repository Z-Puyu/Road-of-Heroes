using System;
using Game.common.characters;
using Game.common.stats;
using Game.util;
using Game.util.events.battle;
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

        private void OnHealed(object sender, EventArgs e) {
            if (e is HealedEvent @event && @event.HandledBy(this.Root)) {
                StatType healedStat = @event.Heal.TargetStat;
                Stat heal = this.Root.Filter(@event.Heal);
                this.Root.Update(healedStat, heal.Value);
            }
        }


        private void OnHeal(object sender, EventArgs e) {
            if (e is HealingEvent @event && @event.HandledBy(this.Root)) {
                this.Publish(new HealedEvent(@event.Target, CombatModule.GenerateHeal(
                    @event.HealTarget, @event.MinHeal, @event.MaxHeal, @event.IsCritical
                )));
            }
        }


        private void OnAttack(object sender, EventArgs e) {
            if (e is AttackEvent @event && @event.HandledBy(this.Root)) {
                double min = @event.MinDmg * @event.DmgMult / 100.0;
                double max = @event.MaxDmg * @event.DmgMult / 100.0;
                Stat dmg = this.Root.Filter(CombatModule.GenerateDamage(
                    @event.DmgType, min, max, @event.IsCritical
                ));
                this.Publish(new AttackedEvent(@event.Target, dmg));
            }
        }

        public void OnAttacked(object sender, EventArgs e) {
            if (e is AttackedEvent @event && @event.HandledBy(this.Root)) {
                this.Root.Update(StatType.Health, -this.Root.Filter(@event.Dmg).Value);
            }
        }

        private static Damage GenerateDamage(StatType dmgType, double min, double max, bool crit) {
            int amount = crit ? (int)Math.Ceiling(max * 1.5) 
                              : Util.Randi((int)Math.Ceiling(min), (int)Math.Ceiling(max));
            return dmgType switch {
                StatType.MeleeDamageDealt => new Damage(StatType.MeleeDamageTaken, amount),
                StatType.RangedDamageDealt => new Damage(StatType.RangedDamageTaken, amount),
                StatType.MagicDamageDealt => new Damage(StatType.MagicDamageTaken, amount),
                _ => new Damage(StatType.MeleeDamageTaken, amount)
            };
        }

        private static Heal GenerateHeal(StatType targetStat, int min, int max, bool crit) {
            int amount = crit ? (int)Math.Ceiling(max * 1.5)
                              : Util.Randi(min, max);
            return new Heal(targetStat, amount);
        }
    }
}