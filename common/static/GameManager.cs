using System;
using Game.common.characters.profession;
using Game.common.characters.race;
using Game.util;
using Godot;
using Godot.Collections;

namespace Game.common.autoload {
    [GlobalClass]
	public partial class GameManager : Node {
		private static GameManager instance;
        [Export] public Array<Vector2I> Resolutions { set; get; } = [];
        [Export] private Array<Profession> Professions { get; set; } = [];
        [Export] private Array<Race> Races { get; set; } = [];
        [ExportGroup("Game constants")]
        [Export] public int MaxPartySize { set; get; } = 10;
        [Export] public Array<Race> DefaultStartingRaces { set; get; } = [];
        private Dictionary<Race.Name, CharacterRandomiser> CharacterRandomisers = [];

		public static GameManager Instance => instance;

        public override void _Ready() {
            GameManager.instance = this;
            foreach (Node child in this.GetChildren()) {
                if (child is CharacterRandomiser randomiser) {
                    this.CharacterRandomisers.Add(randomiser.Race, randomiser);
                }
            }
        }

        public void InitialiseDefaultParty() {
            
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

        public static string RandomName(Race.Name race, bool female = false) {
            return GameManager.instance.CharacterRandomisers[race].RandomName(female);
        }

        public static Texture2D RandomPortrait(Race.Name race, bool isFemale) {
            return GameManager.instance.CharacterRandomisers[race].RandomPortrait(isFemale);
        }

    }
}
