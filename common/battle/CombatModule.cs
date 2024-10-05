using System;
using System.Security.Cryptography;
using Game.common.stats;
using Game.util;

namespace Game.common.battle {
    public class CombatModule {
        private Damage GenerateDamage(StatType dmgType, int dmg) {
            return dmgType switch {
                StatType.MeleeDamageDealt => new Damage(StatType.MeleeDamageTaken, dmg),
                StatType.RangedDamageDealt => new Damage(StatType.RangedDamageTaken, dmg),
                StatType.MagicDamageDealt => new Damage(StatType.MagicDamageTaken, dmg),
                _ => new Damage(StatType.MeleeDamageTaken, dmg)
            };
        }

        public Damage GenerateDamage(StatType dmgType, double min, double max, bool crit) {
            int amount = crit ? (int)Math.Ceiling(max * 1.5) 
                              : Utilities.Randi((int)Math.Ceiling(min), (int)Math.Ceiling(max));
            return this.GenerateDamage(dmgType, amount);
        }

        public Heal GenerateHeal(StatType targetStat, int min, int max, bool crit) {
            int amount = crit ? (int)Math.Ceiling(max * 1.5)
                              : Utilities.Randi(min, max);
            return targetStat switch {
                StatType.Health => new Heal(StatType.HpHeal, amount),
                StatType.Magicka => new Heal(StatType.MagickaHeal, amount),
                StatType.Sanity => new Heal(StatType.SanityHeal, amount),
                StatType.Stamina => new Heal(StatType.StaminaHeal, amount),
                _ => new Heal(StatType.HpHeal, amount)
            };
        }
    }
}