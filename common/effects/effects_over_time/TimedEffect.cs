using System.Collections.Generic;
using System.Threading.Tasks;
using Game.common.characters;
using Godot;

namespace Game.common.effects {
    public abstract partial class TimedEffect : Node {
        [Export] public OverTimeEffect Effect { set; get; }
        protected int time = 0;
        protected readonly Dictionary<int, List<EoT>> effects = [];

        public override void _Ready() {
            this.effects.Clear();
            this.time = 0;
        }

        public abstract void Collect(EoT effect);

        public bool Cure() {
            if (this.effects.Count > 0) {
                this.effects.Clear();
                this.time = 0;
                return true;
            }
            return false;
        }

        public virtual async Task Apply(Actor target) {
            this.time += 1;
            if (this.effects.Remove(this.time) && this.effects.Count == 0) {
                this.time = 0;
            }
        }
    }
}