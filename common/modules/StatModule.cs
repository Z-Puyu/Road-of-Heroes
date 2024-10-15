using System.Collections.Generic;
using Game.common.modifier;
using Game.common.stats;
using Godot;

namespace Game.common.modules {
    /// <summary>
    /// Encapsulates a manager of game object stats.
    /// </summary>
    public partial class StatModule : Module {
        private Dictionary<StatType, Stat> Stats { get; init; } = [];

        /// <summary>
        /// Attempts to retrieve the stat entry of type <paramref name="t"/>. 
        /// The variable <paramref name="value"/> will contain the modified value of the stat retrieved if it exists, and 0 otherwise.
        /// </summary>
        /// <param name="t">The type of the stat entry.</param>
        /// <param name="value">The modified stat value retrieved. Default to 0 if the stat entry does not exist.</param>
        /// <returns>true if the stat entry of type <paramref name="t"/> exists.</returns>
        public bool TryGet(StatType t, out int value) {
            if (this.Stats.TryGetValue(t, out Stat s)) {
                value = s.Value;
                return true;
            }
            value = 0;
            return false;
        }

        /// <summary>
        /// Attempts to update the stat entry of type <paramref name="t"/> by the <paramref name="change"/> value. 
        /// The variable <paramref name="value"/> will contain the modified stat value after the update if it exists, and 0 otherwise.
        /// </summary>
        /// <param name="t">The type of the stat entry.</param>
        /// <param name="change">The change in base stat value.</param>
        /// <param name="value">The modified stat value after the update. Default to 0 if the stat entry does not exist.</param>
        /// <returns>true if the stat entry of type <paramref name="t"/> exists.</returns>
        public bool TryUpdate(StatType t, int change, out int value) {
            if (this.Stats.TryGetValue(t, out Stat s)) {
                s += change;
                this.Stats[t] = s;
                value = s.Value;
                return true;
            }
            value = 0;
            return false;
        }

        /// <summary>
        /// Attempts to add a modifier to its target stat. The variable <paramref name="modifier"/> will contain the updated modifier if the target stat type of the modifier exists, and an identity modifier otherwise.
        /// </summary>
        /// <param name="modifier">The modifier to add.</param>
        /// <returns>true if the target stat of the modifier exists.</returns>
        public bool TryAddModifier(ref Modifier modifier) {
            modifier ??= Modifier.IdentityOf(StatType.MaxHP);
            if (modifier.IsIdentity()) {
                return false;
            }
            if (this.Stats.TryGetValue(modifier.TargetStat, out Stat s)) { 
                s.AddModifier(modifier);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to remove a modifier to its target stat. The variable <paramref name="modifier"/> will contain the updated modifier if the target stat type of the modifier exists, and an identity modifier otherwise.
        /// </summary>
        /// <param name="modifier">The modifier to add.</param>
        /// <returns>true if the target stat of the modifier exists.</returns>
        /// <remarks>publicly, the method removes the modifier by adding its inverse.</remarks>
        public bool TryRemoveModifier(ref Modifier modifier) {
            modifier ??= Modifier.IdentityOf(StatType.MaxHP);
            if (modifier.IsIdentity()) {
                return false;
            }
            if (this.Stats.TryGetValue(modifier.TargetStat, out Stat s)) { 
                s.AddModifier(-modifier);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to add a new stat entry. By right we should not call this method.
        /// </summary>
        /// <param name="s">The stat entry.</param>
        /// <returns>true if there is no stat entry with the same type.</returns>
        public bool TryAdd(in Stat s) {
            return this.Stats.TryAdd(s.Type, s);
        }

        /// <summary>
        /// Checks if the stat entry of type <paramref name="t"/> exists. By right we should not call this method.
        /// </summary>
        /// <param name="t">The type of the stat entry.</param>
        /// <returns>true if the stat entry of type <paramref name="t"/> exists.</returns>
        public bool Contains(StatType t) {
            return this.Stats.ContainsKey(t);
        }

        protected override void ConnectEvents()
        {
            throw new System.NotImplementedException();
        }
    }
}