namespace Game.common.stats {
    public enum StatType {
        // Base Character Stats
        Agility,
        Speed,
        Strength,
        Perception,
        Precision,
        // Resistance Stats
        BleedResist,
        PoisonResist,
        BurnResist,
        BlightResist,
        StunResist,
        FrenzyResist,
        BuffResist,
        DebuffResist,
        StealthResist,
        WardedResist,
        // Physical Conditions
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
        HpHealingGiven,
        MagickaHealingGiven,
        SanityHealingGiven,
        StaminaHealingGiven,
        HpHealingReceived,
        MagickaHealingReceived,
        SanityHealingReceived,
        StaminaHealingReceived,
        BleedChance,
        BlightChance,
        BurnChance,
        PoisonChance,
        FrenzyChance,
        StunChance,
        BuffChance,
        DebuffChance,
    }
}