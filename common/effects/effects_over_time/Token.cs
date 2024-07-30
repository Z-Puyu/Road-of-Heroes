using System.Collections.Generic;
using System.Threading.Tasks;
using Game.common.autoload;
using Godot;

namespace Game.common.effects.eot {
    [GlobalClass]
    public abstract partial class Token : TextureRect {
        [Export] public EoT.Effect Effect { set; get; }
        private CharacterCard root;
        protected int expiry = 0;
        protected int time = 0;
        protected readonly Dictionary<int, List<EoT>> eots = [];

        public CharacterCard Root { protected get => root; set => root = value; }

        public override void _Ready() {
            this.Reset();
        }

        public abstract void Compose(EoT eot);

        public void Cure() {
            this.Reset();
        }

        protected virtual void Reset() {
            this.expiry = 0;
            this.time = 0;
            this.eots.Clear();
            this.Hide();
        }

        public virtual async Task Apply() {
            await this.Animate();
            this.time += 1;
            if (this.time == this.expiry) {
                this.Reset();
            }
        }

        protected async Task Animate() {
            Vector2 scale = this.Scale;
            await AnimationManager.Animate(this, "scale", scale * 2, 0.25, Tween.EaseType.Out);
            await AnimationManager.Animate(this, "scale", scale, 0.25, Tween.EaseType.Out, 0.5);
        }
    }
}