using Game.common.autoload;
using Godot;
using Godot.Collections;

namespace Game.ui.characters.player {
	public partial class PartyUI : GridContainer {
		[Export] private Array<NodePath> avatars = [];
		private readonly Array<CharacterAvatar> portraits = [];

        public override void _Ready() {
			for (int i = 0; i < this.avatars.Count; i += 1) {
				this.portraits.Add(this.GetNode<CharacterAvatar>(this.avatars[i]));
			}
			for (int i = 0; i < this.avatars.Count; i += 1) {
				this.portraits[i].Set(PlayerManager.Combatants[i]);
			}
        }
	}
}
