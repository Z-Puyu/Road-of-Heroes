using System;
using Game.common.characters;
using Game.common.effects;
using Game.UI.characters;
using Godot;

public partial class PlayerCard : CharacterCard, IEffectEmitter, IEffectReceiver {
	private readonly static PackedScene scene = GD.Load<PackedScene>("res://assets/interface/PlayerCard.tscn");
    [Export] private NodePath Profession { set; get; }
	[Export] private NodePath RaceLabel { set; get; }
	[Export] private NodePath LevelLabel { set; get; }

	public new PlayerCharacter Character => this.character is PlayerCharacter c ? c : null;

    public override void _Ready() {
		base._Ready();
		foreach (Node node in this.GetNode<VBoxContainer>(this.Bars).GetChildren()) {
			if (node is FancyProgressBar bar && this.character.Get(bar.DataType, out Stat stat)) {
				bar.Init(stat, this);
			}
		}
		if (this.Character.Profession != null) {
			this.GetNode<TextureRect>(this.Profession).Texture = this.Character.Profession.Icon;
		}
		this.GetNode<Label>(this.RaceLabel).Text = this.Character.Race.RaceName.ToString();
		this.GetNode<Label>(this.LevelLabel).Text = $"LV. {this.Character.LevelAsInt}";
	}

	public static PlayerCard Of(PlayerCharacter character) {
		PlayerCard card = PlayerCard.scene.Instantiate<PlayerCard>();
		card.character = character;
		return card;
	}
}
