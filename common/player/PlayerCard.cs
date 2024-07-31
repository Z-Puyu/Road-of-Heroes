using System;
using Game.common.characters;
using Game.common.effects;
using Game.UI.characters;
using Godot;

public partial class PlayerCard : CharacterCard, IEffectEmitter, IEffectReceiver {
	private readonly static PackedScene scene = GD.Load<PackedScene>("res://assets/interface/PlayerCard.tscn");
	private PlayerCharacter character;
    [Export] private NodePath Profession { set; get; }
	[Export] private NodePath RaceLabel { set; get; }
	[Export] private NodePath LevelLabel { set; get; }

	public new PlayerCharacter Character => character;

    public override void _Ready() {
		base._Ready();
		foreach (Node node in this.GetNode<VBoxContainer>(this.Bars).GetChildren()) {
			if (node is FancyProgressBar bar && this.character.Get(bar.DataType, out Stat stat)) {
				bar.Init(stat);
			}
		}
		this.GetNode<TextureRect>(this.Avatar).Texture = this.character.Avatar;
		this.GetNode<Label>(this.NameLabel).Text = this.character.Name;
		if (this.character.Profession != null) {
			this.GetNode<TextureRect>(this.Profession).Texture = this.character.Profession.Icon;
		}
		this.GetNode<Label>(this.RaceLabel).Text = this.character.Race.RaceName.ToString();
		this.GetNode<Label>(this.LevelLabel).Text = $"LV. {this.character.LevelAsInt}";
	}

	public static PlayerCard Of(PlayerCharacter character) {
		PlayerCard card = PlayerCard.scene.Instantiate<PlayerCard>();
		card.character = character;
		return card;
	}
}
