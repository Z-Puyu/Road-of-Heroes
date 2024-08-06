using System;
using Game.UI.characters;
using Godot;

namespace Game.common.characters.enemies {
	public partial class EnemyCard : CharacterCard {
		private readonly static PackedScene scene = GD.Load<PackedScene>("res://assets/interface/EnemyCard.tscn");

		public new EnemyCharacter Character => this.character is EnemyCharacter enemy ? enemy : null;

        public override void _Ready() {
			base._Ready();
			foreach (Node node in this.GetNode<VBoxContainer>(this.Bars).GetChildren()) {
				if (node is FancyProgressBar bar && this.character.Get(bar.DataType, out Stat stat)) {
					bar.Init(stat, this);
				}
			}
		}

		public static EnemyCard Of(EnemyCharacter character) {
			EnemyCard card = EnemyCard.scene.Instantiate<EnemyCard>();
			card.character = character;
			return card;
		}
	}
}
