using Game.common.effects.eot;
using Godot;

namespace Game.ui.characters {
	public partial class ResistanceLabel : VBoxContainer {
		[Export] public EoT.Effect Effect { set; get; }
		[Export] private NodePath Value { set; get; }
		[Export] private NodePath Icon { set; get; }
		[Export] private Texture2D Texture { set; get; }

        public override void _Ready() {
            this.GetNode<TextureRect>(this.Icon).Texture = this.Texture;
        }

        public void Set(int value) {
			this.GetNode<Label>(this.Value).Text = $"{value}%";
		}
	}
}
