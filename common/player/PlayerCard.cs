using System;
using Game.common.characters;
using Game.common.effects;
using Game.UI.characters;
using Godot;

public partial class PlayerCard : CharacterCard, IEffectEmitter, IEffectReceiver {
	private readonly static PackedScene scene = GD.Load<PackedScene>("res://assets/interface/PlayerCard.tscn");
	private PlayerCharacter character;
    [Export] private TextureRect ProfessionIcon { set; get; }
	[Export] private Label RaceLabel { set; get; }
	[Export] private Label LevelLabel { set; get; }

	public new PlayerCharacter Character => character;

    public override void _Ready() {
		base._Ready();
		foreach (Node node in this.Bars.GetChildren()) {
			if (node is FancyProgressBar bar && this.character.Get(bar.DataType, out Stat stat)) {
				bar.Init(stat);
			}
		}
		this.Avatar.Texture = this.character.Avatar;
		this.NameLabel.Text = this.character.Name;
		this.ProfessionIcon.Texture = this.character.Profession.Icon;
		this.RaceLabel.Text = this.character.Race.RaceName.ToString();
		this.LevelLabel.Text = this.character.Level.ToString();
	}

	public static PlayerCard Of(PlayerCharacter character) {
		PlayerCard card = PlayerCard.scene.Instantiate<PlayerCard>();
		card.character = character;
		return card;
	}
}
