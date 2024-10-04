using System;
using Game.common.stats;
using Game.util;

namespace Game.common.characters.module {
    public class CombatModule {
        private Damage GenerateDamage(StatType dmgType, int dmg) {
            return dmgType switch {
                StatType.MeleeDamageDealt => new Damage(StatType.MeleeDamageTaken, dmg),
                StatType.RangedDamageDealt => new Damage(StatType.RangedDamageTaken, dmg),
                StatType.MagicDamageDealt => new Damage(StatType.MagicDamageTaken, dmg),
                _ => new Damage(StatType.MeleeDamageTaken, dmg)
            };
        }
        
        public Damage GenerateDamage(StatType dmgType, int strength, bool crit, int multiplier) {
            Stat dmg = (crit ? new Damage(dmgType, (int)Math.Ceiling(2 * strength * 1.5))
                            : new Damage(dmgType, Utilities.Randi(strength, 2 * strength))) 
                        * (multiplier / 100.0, 0, 0);
            return this.GenerateDamage(dmgType, dmg.Value);
        }

        public Damage GenerateDamage(Damage min, Damage max, bool crit) {
            if (min.Type != max.Type) {
                throw new ArgumentException();
            }
            int amount = crit ? (int)Math.Ceiling(max.Value * 1.5) 
                              : Utilities.Randi(min.Value, max.Value);
            return this.GenerateDamage(min.Type, amount);
        }
    }
}