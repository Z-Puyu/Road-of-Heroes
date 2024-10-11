namespace Game.common.stats {
    /// <summary>
    /// A collection of all types of stats used in the game.
    /// </summary>
    public enum ModifiableValueType {
        // Base Character ModifiableValues
        Agility,
        Speed,
        Strength,
        Perception,
        Precision,
        // Resistance ModifiableValues
        BleedResist,
        PoisonResist,
        BurnResist,
        BlightResist,
        StunResist,
        // Physical Conditions (Capped)
        Health,
        Magicka,
        Sanity,
        Stamina,
        MaxHP,
        MaxMagicka,
        MaxSanity,
        MaxStamina,
        // Stats Unrelated to Characters
        HpCost,
        MagickaCost,
        SanityCost,
        StaminaCost,
        MeleeDamageDealt,
        MeleeDamageTaken,
        RangedDamageDealt,
        RangedDamageTaken,
        MagicDamageDealt,
        MagicDamageTaken,
        HpHeal,
        MagickaHeal,
        SanityHeal,
        StaminaHeal,
        BleedChance,
        BlightChance,
        BurnChance,
        PoisonChance,
        FrenzyChance,
        StunChance,
    }
}