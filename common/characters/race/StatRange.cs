using Game.common.stats;
using Godot;

namespace Game.common.characters.race {
    [GlobalClass]
    public partial class StatRange : Resource {
        [Export] public StatType Type { private set; get; }
        [Export(PropertyHint.Range, "0,100")] public int Max { private set; get; } = 0;
        [Export(PropertyHint.Range, "0,100")] public int Min { private set; get; } = 0;

        public StatRange() {}

        public StatRange(StatType type) {
            this.Type = type;
        }
    }
}