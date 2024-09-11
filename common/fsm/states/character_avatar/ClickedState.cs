using Game.util;
using Game.util.events.party;
using Godot;

namespace Game.common.fsm.states {
	public partial class ClickedState : State {
        public ClickedState() : base(Type.AvatarClicked) {}

        public override void OnInput(InputEvent e) {
			if (this.FSM.Root is CharacterAvatar avatar) {
				if (e.IsActionReleased("left_click")) {
					this.Publish(new DisplayCharacterDetailEvent(avatar.Character));
					this.FSM.TransitionTo(Type.AvatarIdle);
				} else if (e is InputEventMouseMotion) {
					// this.FSM.TransitionTo(Type.AvatarDragging);
				}
			}
		}
	}
}
