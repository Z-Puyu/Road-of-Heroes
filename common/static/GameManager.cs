using Game.common.characters.race;
using Game.util.errors;
using Godot;
using Godot.Collections;

namespace Game.common.autoload {
    [GlobalClass]
	public partial class GameManager : Node {
		private static GameManager Instance { set; get; }
        [Export] public Array<Vector2I> Resolutions { set; get; } = [];
        [Export] private Array<Race> Races { get; set; } = [];
        private static Dictionary<Race.Species, CharacterManager> CharacterManagers { set; get; } = [];

        private static Node2D world;

        public static Node2D World { get => world; set => world = value; }

        public GameManager() {
            if (GameManager.Instance != null) {
                throw new SingletonException(typeof(GameManager));
            }
            GameManager.Instance = this;
        }

        public override void _Ready() {
            GameManager.Instance ??= this;
            foreach (Node child in this.GetChildren()) {
                if (child is CharacterManager manager) {
                    GameManager.CharacterManagers[manager.Race] = manager;
                }
            }
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
            return GameManager.Instance.Races.PickRandom();
        }

        public static string RandomName(Race race, bool isFemale) {
            return GameManager.CharacterManagers[race.Name].RandomName(isFemale);
        }
    }
}
