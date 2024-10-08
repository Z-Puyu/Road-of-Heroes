using Game.common.characters;
using Game.common.characters.skills;
using Game.util;
using Game.util.events.battle;
using Godot;

namespace Game.ui.characters.player {
	public partial class BattleSkillButton : TextureButton {
		[Export] private NodePath Tooltip { set; get; }
		[Export] private NodePath Icon { set; get; }
		[Export] private NodePath Label { set; get; }
		[Export] private NodePath Desc { set; get; }
		[Export] private Skill Skill { set; get; }

		private Vector2 anchor;
		private PlayerCharacter character;

        public override void _Ready() {
			this.GetNode<AspectRatioContainer>(this.Tooltip).Hide();
            this.MouseEntered += this.OnMouseEntered;
			this.MouseExited += this.OnMouseExited;
			this.Toggled += this.OnToggled;
        }

		public bool IsLoaded() {
			return this.Skill != null;
		}

        private void OnToggled(bool toggledOn) {
            if (toggledOn) {
				this.Publish(new SkillReadyEvent(this.Skill, this.character));
			} else {
				this.Publish(new SkillUnequipedEvent(this.Skill));
			}
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
			if (this.Skill == null && tooltip.Visible) {
				return;
			}
			if (this.Skill.IsRacialSkill) {
				this.GetNode<Label>(this.Label).Show();
			} else {
				this.GetNode<Label>(this.Label).Hide();
			}
			this.GetNode<Label>(this.Desc).Text = this.Skill.ToDesc(this.character);
			tooltip.Show();
			this.anchor = this.GetGlobalTransformWithCanvas().Origin;
			tooltip.GlobalPosition = this.anchor - new Vector2(tooltip.Size.X, 0);
        }

        public void Load(Skill skill, PlayerCharacter character) {
			this.Skill = skill;
			this.character = character;
			this.GetNode<TextureRect>(this.Icon).Texture = skill.Icon;
		}
	}
}
