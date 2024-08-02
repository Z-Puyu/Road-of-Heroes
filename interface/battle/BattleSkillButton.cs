using Game.common.characters;
using Game.common.characters.skills;
using Godot;

namespace Game.ui.characters.player {
	public partial class BattleSkillButton : TextureButton {
		[Export] private NodePath Tooltip { set; get; }
		[Export] private NodePath Icon { set; get; }
		[Export] private NodePath Label { set; get; }
		[Export] private NodePath Desc { set; get; }

        private Skill skill;
		private Vector2 anchor;
		private PlayerCharacter character;

        public override void _Ready() {
			this.GetNode<AspectRatioContainer>(this.Tooltip).Hide();
            this.MouseEntered += this.OnMouseEntered;
			this.MouseExited += this.OnMouseExited;
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

        public void Load(Skill skill, PlayerCharacter character) {
			this.skill = skill;
			this.character = character;
			this.GetNode<TextureRect>(this.Icon).Texture = skill.Icon;
		}
	}
}
