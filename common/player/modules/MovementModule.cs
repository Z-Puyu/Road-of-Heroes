using System.Collections.Generic;
using System.Threading;
using Game.common.autoload;
using Godot;

namespace Game.common.player.modules {
    [GlobalClass]
    public partial class MovementModule : Node, IModule {
        [Export] private Player Root { set; get; }
        private readonly Queue<Vector2> path = [];
        private Godot.Timer timer;

        public override void _Ready() {
            this.timer = this.GetNode<Godot.Timer>("./Timer");
            this.timer.Timeout += this.OnTimerTimeout;
        }

        private async void OnTimerTimeout() {
            Vector2 next = this.path.Dequeue();
            if (this.path.Count == 0) {
                this.timer.Stop();
            }
            await AnimationManager.Animate(
                this.Root, "position", next, 0.2, Tween.EaseType.InOut
            );
        }

        public void Work() {
            this.path.Clear();
            foreach (Vector2 point in this.Root.Path) {
                this.path.Enqueue(point);
            }
            this.timer.Start();
        }
    }
}