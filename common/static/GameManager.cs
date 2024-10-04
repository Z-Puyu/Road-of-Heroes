using Game.common.characters.race;
using Godot;
using Godot.Collections;

namespace Game.common.autoload {
    [GlobalClass]
	public partial class GameManager : Node {
		private static GameManager instance;
        [Export] public Array<Vector2I> Resolutions { set; get; } = [];
        [Export] private Array<Race> Races { get; set; } = [];
        private static Node2D world;

		public static GameManager Instance => instance;
        public static Node2D World { get => world; set => world = value; }


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

        public static Race RandomRace() {
            return GameManager.instance.Races.PickRandom();
        }
    }
}
