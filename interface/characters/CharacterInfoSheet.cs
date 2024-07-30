using System;
using System.Collections.Generic;
using Game.common.autoload;
using Game.common.characters;
using Game.common.characters.skills;
using Game.common.effects.eot;
using Game.ui.characters;
using Game.ui.characters.player;
using Game.util;
using Game.util.events.party;
using Godot;

public partial class CharacterInfoSheet : Panel {
	private static Vector2 position;
	private readonly Dictionary<Stat.Category, StatLabel> stats = [];
	private readonly Dictionary<EoT.Effect, ResistanceLabel> resistances = [];
	[Export] private NodePath Bars { set; get; }
	[Export] private NodePath Left { set; get; }
	[Export] private NodePath Resistance { set; get; }
	[Export] private NodePath Portrait { set; get; }
	[Export] private NodePath Label { set; get; }
	[Export] private NodePath CloseButton { set; get; }
	[Export] private NodePath Skills { set; get; }

    public override void _Ready() {
        foreach (Node child in this.GetNode<VBoxContainer>(this.Left).GetChildren()) {
			if (child is StatLabel stat) {
				this.stats.Add(stat.Stat, stat);
			}
		}
		foreach (Node child in this.GetNode<HBoxContainer>(this.Resistance).GetChildren()) {
			if (child is ResistanceLabel resist) {
				this.resistances.Add(resist.Effect, resist);
			}
		}
		this.GetNode<Button>(this.CloseButton).Pressed += this.OnClose;
		this.Subscribe<DisplayCharacterDetailEvent>(this.OnDisplayCharacter);
		CharacterInfoSheet.position = this.GlobalPosition;
		this.Hide();
    }

	private async void OnClose() {
		await AnimationManager.Animate(
			this, "global_position", 
			new Vector2(this.GetViewportRect().Size.X + this.Size.X, this.GlobalPosition.Y), 
			1, Tween.EaseType.In
		);
		this.Hide();
	}

	private void OnDisplayCharacter(object sender, EventArgs e) {
		if (e is DisplayCharacterDetailEvent @event) {
			if (@event.Enemy == null && @event.Character != null) {
				this.Display(@event.Character);
			}
		}
	}

	public async void Display(PlayerCharacter character) {
		this.GetNode<TextureRect>(this.Portrait).Texture = character.Avatar;
		this.GetNode<Label>(this.Label).Text = $"{character.Name}\n{character.Level} {character.Race.ToAdj()} {character.Profession}";	
		foreach (KeyValuePair<Stat.Category, StatLabel> pair in this.stats) {
			pair.Value.Set(character.Get(pair.Key));
		}
		foreach (Node child in this.GetNode<VBoxContainer>(this.Bars).GetChildren()) {
			if (child is StatBar bar && character.Get(bar.StatType, out Stat value)) {
				bar.Init(value);
			}
		}
		foreach (KeyValuePair<EoT.Effect, ResistanceLabel> pair in this.resistances) {
			pair.Value.Set(character.Get((Stat.Category)pair.Key));
		}
		GridContainer skills = this.GetNode<GridContainer>(this.Skills);
		int idx = 0;
		foreach (Skill skill in character.Skills.Keys) {
			SkillButton button = skills.GetChild<SkillButton>(idx);
			button.Load(skill);
			if (character.ActiveSkills.Contains(skill)) {
				button.ButtonPressed = true;
			}
		}
		if (!this.Visible) {
			this.GlobalPosition = new Vector2(this.GetViewportRect().Size.X + this.Size.X, this.GlobalPosition.Y);
			this.Show();
			await AnimationManager.Animate(
				this, "global_position", CharacterInfoSheet.position, 1, Tween.EaseType.Out
			);
		}
	}
}
