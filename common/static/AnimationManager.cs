using Game.util;
using Godot;
using System.Threading.Tasks;

namespace Game.common.autoload {
	[GlobalClass]
	public partial class AnimationManager : Node {
		private static AnimationManager instance;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready() {
			AnimationManager.instance = this;
		}

		public static async Task Animate(
			Node node, NodePath property, Variant finalVal, double duration, Tween.EaseType ease, 
			double delay = 0, bool deleteNodeUponFinish = false
		) {
			Tween tween = AnimationManager.instance.CreateTween();
			tween.TweenProperty(node, property, finalVal, duration).SetDelay(delay);
			tween.SetEase(ease);
			tween.Play();
			await tween.ToSignal(tween, Tween.SignalName.Finished);
			if (deleteNodeUponFinish) {
				node.Recycle();
			}
		}
	}
}
