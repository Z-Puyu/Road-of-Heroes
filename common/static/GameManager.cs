using Godot;
using Godot.Collections;

namespace Game.common.autoload {
	public partial class GameManager : Node {
		private static GameManager instance;
        [Export] public Array<Vector2I> Resolutions { set; get; } = [];

		public static GameManager Instance => instance;

        public override void _Ready() {
            GameManager.instance = this;
        }

        public static T Instantiate<T>(PackedScene scene, Vector2 position, Node parent = null) 
            where T : Node {
            T @object = scene.Instantiate<T>();
            if (parent != null) {
                parent.AddChild(@object);
            }
            if (@object is Node2D node) {
                node.GlobalPosition = position;
            }
            return @object;
        }
    }
}
