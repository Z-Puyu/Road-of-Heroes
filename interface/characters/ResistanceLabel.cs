using Game.common.effects.eot;
using Godot;

namespace Game.ui.characters {
	public partial class ResistanceLabel : VBoxContainer {
		[Export] public EoT.Effect Effect { set; get; }
		[Export] private Label Value { set; get; }

		public void Set(int value) {
			this.Value.Text = $"{value}%";
		}
	}
}
