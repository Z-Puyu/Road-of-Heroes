using Game.common.characters.race;
<<<<<<< HEAD:common/static/CharacterRandomiser.cs
using Game.util;
=======
using Game.util.errors;
>>>>>>> e50a7f5edd12946b0af396b056629f5c7b368333:common/static/CharacterManager.cs
using Game.util.math;
using Godot;
using Godot.Collections;

namespace Game.common.autoload {
	[GlobalClass]
	public partial class CharacterManager : Node {
		[Export] public Race.Species Race { set; get; }
		[Export] private Array<Texture2D> Males { set; get; } = [];
		[Export] private Array<Texture2D> Females { set; get; } = [];
		[Export] private string MaleNamePath { set; get; }
		[Export] private string FemaleNamePath { set; get; }
		[Export] private string NeutralNamePath { set; get; } = "";
		private readonly System.Collections.Generic.Dictionary<int, string> surnames = [];
		private readonly System.Collections.Generic.Dictionary<int, string> femaleNames = [];
		private readonly System.Collections.Generic.Dictionary<int, string> maleNames = [];

        public override void _Ready() {
			// Load male names.
			string[] allMaleNames = FileAccess.Open(
				this.MaleNamePath, FileAccess.ModeFlags.Read
			).GetAsText(true).Split('\n');
			foreach (string name in allMaleNames) {
				string[] tokens = name.Split(' ');
				if (tokens.Length == 2) {
					if (!this.surnames.ContainsValue(tokens[1])) {
						this.surnames.Add(this.surnames.Count, tokens[1]);
					}
				}
				if (!this.maleNames.ContainsValue(tokens[0])) {
					this.maleNames.Add(this.maleNames.Count, tokens[0]);
				}
			}
			// Load female names.
			string[] allFemaleNames = FileAccess.Open(
				this.FemaleNamePath, FileAccess.ModeFlags.Read
			).GetAsText(true).Split('\n');
			foreach (string name in allFemaleNames) {
				string[] tokens = name.Split(' ');
				if (tokens.Length == 2) {
					if (!this.surnames.ContainsValue(tokens[1])) {
						this.surnames.Add(this.surnames.Count, tokens[1]);
					}
				}
				if (!this.femaleNames.ContainsValue(tokens[0])) {
					this.femaleNames.Add(this.femaleNames.Count, tokens[0]);
				}
			}
			// Load neutral names.
			if (this.NeutralNamePath.Length > 0) {
				string[] allNeutralNames = FileAccess.Open(
					this.NeutralNamePath, FileAccess.ModeFlags.Read
				).GetAsText(true).Split('\n');
				foreach (string name in allNeutralNames) {
					string[] tokens = name.Split(' ');
					if (tokens.Length == 2) {
						if (!this.surnames.ContainsValue(tokens[1])) {
							this.surnames.Add(this.surnames.Count, tokens[1]);
						}
					}
					if (!this.maleNames.ContainsValue(tokens[0])) {
						this.maleNames.Add(this.maleNames.Count, tokens[0]);
					}
					if (!this.femaleNames.ContainsValue(tokens[0])) {
						this.femaleNames.Add(this.femaleNames.Count, tokens[0]);
					}
				}
			}
        }

        public Texture2D RandomPortrait(bool female = false) {
			return female ? this.Females[MathUtil.Randi(0, this.Females.Count - 1)] 
						  : this.Males[MathUtil.Randi(0, this.Males.Count - 1)];
		}

		public string RandomName(bool female) {
			string name = female ? this.femaleNames[MathUtil.Randi(0, this.femaleNames.Count - 1)]
								 : this.maleNames[MathUtil.Randi(0, this.maleNames.Count - 1)];
			return this.surnames.Count > 0 
					? name + " " + this.surnames[MathUtil.Randi(0, this.surnames.Count - 1)] 
					: name;
		}
	}
}

