using System;
using Godot;

namespace Game.common.modifier {
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