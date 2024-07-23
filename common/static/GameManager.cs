using Godot;
using Godot.Collections;

namespace Game.common.autoload {
	public partial class GameManager : Node {
		private static GameManager node;
        [Export] public Array<Vector2I> Resolutions { set; get; } = [];

		public static GameManager Node => node;

        public override void _Ready() {
            GameManager.node = this.GetNode<GameManager>("/root/GameManager");
        }
    }
}
