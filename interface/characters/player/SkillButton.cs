using System;
using Game.common.characters;
using Game.common.characters.skills;
using Game.util;
using Game.util.events.battle;
using Godot;

namespace Game.ui.characters.player {
	public partial class SkillButton : TextureButton {
		[Export] private NodePath Tooltip { set; get; }
		[Export] private NodePath Icon { set; get; }
		[Export] private NodePath Label { set; get; }
		[Export] private NodePath Desc { set; get; }

        private Skill skill;
		private Vector2 anchor;
		private PlayerCharacter character;
		private int idx;

        public override void _Ready() {
			this.GetNode<AspectRatioContainer>(this.Tooltip).Hide();
            this.MouseEntered += this.OnMouseEntered;
			this.MouseExited += this.OnMouseExited;
			this.Toggled += this.OnToggled;
			this.Subscribe<SkillChangedEvent>(this.OnSkillChanged);
        }

        private void OnSkillChanged(object sender, EventArgs e) {
            if (e is SkillChangedEvent @event && @event.Character == this.character) {
				if (this.skill.IsRacialSkill) {
					this.Disabled = this.character.LevelAsInt + 1 / 2 < this.idx;
				} else {
					this.Disabled = this.character.ActiveSkills.Count == 5;
				}
			}
        }


        private void OnToggled(bool toggledOn) {
            if (toggledOn) {
				if (!this.character.ActiveSkills.Contains(this.skill)) {
					this.character.ActiveSkills.Add(this.skill);
				}
			} else {
				this.character.ActiveSkills.Remove(this.skill);
			}
			this.Publish(new SkillChangedEvent(this.character));
        }

        private void OnMouseExited() {
			AspectRatioContainer tooltip = this.GetNode<AspectRatioContainer>(this.Tooltip);
			tooltip.GlobalPosition = this.anchor;
			tooltip.Hide();
        }


        private void OnMouseEntered() {
			if (this.Disabled) {
				return;
			}
			AspectRatioContainer tooltip = this.GetNode<AspectRatioContainer>(this.Tooltip);
			if (this.skill == null && tooltip.Visible) {
				return;
			}
			if (this.skill.IsRacialSkill) {
				this.GetNode<Label>(this.Label).Show();
			} else {
				this.GetNode<Label>(this.Label).Hide();
			}
			this.GetNode<Label>(this.Desc).Text = this.skill.ToDesc(this.character);
			tooltip.Show();
			this.anchor = this.GetGlobalTransformWithCanvas().Origin;
			tooltip.GlobalPosition = this.anchor - new Vector2(tooltip.Size.X, 0);
        }

        public void Load(Skill skill, PlayerCharacter character, int idx) {
			this.skill = skill;
			this.character = character;
			this.idx = idx;
			this.GetNode<TextureRect>(this.Icon).Texture = skill.Icon;
			if (character.ActiveSkills.Contains(skill)) {
				this.ButtonPressed = true;
			} else if (character.ActiveSkills.Count == 5) {
				this.Disabled = true;
			}
		}
	}
}
