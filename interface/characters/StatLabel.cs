using Game.common.characters;
using Godot;

namespace Game.ui.characters {
	public partial class StatLabel : Label {
		[Export] public Stat.Category Stat { set; get; }
		[Export] private string DisplayName { set; get; }

		public void Set(int value) {
			this.Text = $"{this.DisplayName}: {value}";
		}
	}
}
