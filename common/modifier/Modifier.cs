using System;
using Godot;
using MonoCustomResourceRegistry;

namespace Game.common.modifier {
    [RegisteredType(nameof(Modifier), "", nameof(Resource)), GlobalClass]
    public abstract partial class Modifier : Resource {
        [Export] private int TimeToLast = 0;
        public Action OnExpire { get; set; }
        [Export] public bool UsePercentage { get; set; } = false;
        [Export] public int Value { set; get; } = 1;

        public Modifier() {}

        public Modifier(bool usingPercentage) {
            UsePercentage = usingPercentage;
        }
    }
}