using Game.common.characters.profession;
using Game.common.characters.race;
using Game.util;
using Godot;
using Godot.Collections;

namespace Game.common.autoload {
	public partial class GameManager : Node {
		private static GameManager instance;
        [Export] public Array<Vector2I> Resolutions { set; get; } = [];
        [Export] private Array<Profession> Professions { get; set; } = [];
        [Export] private Array<Race> Races { get; set; } = [];
        private Dictionary<Race.Name, Array<string>> Names = [];

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

        public static Profession RandomProfession() {
            int idx = Utilities.Randi(0, GameManager.instance.Professions.Count - 1);
            return GameManager.instance.Professions[idx];
        }

        public static Race RandomRace() {
            int idx = Utilities.Randi(0, GameManager.instance.Races.Count - 1);
            return GameManager.instance.Races[idx];
        }

        public static string RandomName(Race.Name race) {
            int idx = Utilities.Randi(0, GameManager.instance.Names[race].Count - 1);
            return GameManager.instance.Names[race][idx];
        }
    }
}
