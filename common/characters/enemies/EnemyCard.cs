using Game.UI.characters;
using Godot;

namespace Game.common.characters.enemies {
	public partial class EnemyCard : CharacterCard {
		private readonly static PackedScene scene = GD.Load<PackedScene>("res://assets/interface/EnemyCard.tscn");
		private EnemyCharacter character;

		public new EnemyCharacter Character => character;

        public override void _Ready() {
			foreach (Node node in this.GetNode<VBoxContainer>(this.Bars).GetChildren()) {
				if (node is FancyProgressBar bar && this.character.Get(bar.DataType, out Stat stat)) {
					bar.Init(stat);
				}
			}
			this.GetNode<TextureRect>(this.Avatar).Texture = this.character.Avatar;
			this.GetNode<Label>(this.NameLabel).Text = this.character.Name;
		}

		public static EnemyCard Of(EnemyCharacter character) {
			EnemyCard card = EnemyCard.scene.Instantiate<EnemyCard>();
			card.character = character;
			return card;
		}
	}
}
